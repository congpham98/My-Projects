import codecs
import os
import re
import json
import math
import collections
from itertools import islice
import threading
def getstopwords(data_path):
    stopword_file = codecs.open(data_path,"r","utf-8")
    content = stopword_file.read().split('\n')
    stopword_file.close()
    return content

def build_inverted_index(data_path, stopwords):
    f = os.listdir(data_path)
    dictionary = {}
    for filename in f:
        link = codecs.open(data_path + filename,"r","utf-8")
        split_words=link.read().lower()
        link.close()
        split_words=re.sub(r'[-|?|$|.|!|"|,|(|)|/|_|\'|`|*|+|@|#|%|^|&|[|]|{|}|;|:|<|>|،|、|…|⋯|᠁|ฯ|‹|›|«|»|‘|’|“|”|‱|‰|±|∓|¶|‴|§|‖|¦|©|🄯|℗|®|℠|™|]',' ', split_words)
        split_words=re.sub(r"\b(" + "|".join(stopwords) + ")\\W",r" ",split_words)
        new=list(split_words.split())
        if(new[-1] in stopwords):
            del new[-1]
        for word in new:
            if (word not in dictionary):
                dictionary[word] = {}
            if (filename not in dictionary[word]):
                dictionary[word][filename]=1
            else:
                dictionary[word][filename]+=1
        with codecs.open("inverted_index.txt","w","utf-8") as fp:
            json.dump(dictionary,fp)

def compute_tf_idf(path, words_in_InvertedIndex):
    tf_idf = {}
    f=os.listdir(path)
    for word in words_in_InvertedIndex:
        tf_idf[word]={}
        idf=1.0+math.log10(float( len(f)/len(words_in_InvertedIndex[word]) ))
        for filename in f:
                if (filename in words_in_InvertedIndex[word]):
                    link = codecs.open(path+filename,"r","utf-8")
                    content=link.read().lower()
                    link.close()
                    content=re.sub(r'[-|?|$|.|!|"|,|(|)|/|_|\'|`|*|+|@|#|%|^|&|[|]|{|}|;|:|<|>|،|、|…|⋯|᠁|ฯ|‹|›|«|»|‘|’|“|”|‱|‰|±|∓|¶|‴|§|‖|¦|©|🄯|℗|®|℠|™|]',r' ', content)
                    newlist=list(content.split())
                    tf=words_in_InvertedIndex[word][filename]/len(newlist)
                    tf_idf[word][filename]=tf*idf
    with codecs.open("tf_idf.txt","w","utf-8") as fp:
        json.dump(tf_idf,fp)
        
def compute_tf_idf_query(path, words, query):#words = tf_idf
	tf_idf_query = {}
	file = os.listdir(path)
	file_relevance = []
	for item in list(query.lower().split()):
		if(item in words):
			for key in words[item]:
				if(key not in file_relevance):
					file_relevance.append(key)
			x = 0.0
			y = 1.0 + math.log10(float(len(file) / len(words[item])))
			for new in list(query.lower().split()):
				if(new == item):
					x += 1.0
			x /= len(query.split())
			tf_idf_query[item] = x * y
    #---------------------------------------------------------------        
	stopwords = getstopwords("vn.txt")
	stopwords = re.compile(r"\b(" + "|".join(stopwords) + ")\\W")
	cosine = {}
	for filename in file_relevance:
		file1 = codecs.open(path + filename, 'r', 'utf-8')
		line = file1.readline()
        
		while(len(line) <= 2):#mot so file co dong dau tien trong
			line = file1.readline()
		file1.close()
		line = re.sub(stopwords,r' ', line.lower())
		line = (re.sub(r'[-|?|$|.|!|"|,|(|)|/|_|\'|`|*|+|@|#|%|^|&|[|]|{|}|;|:|<|>|،|、|…|⋯|᠁|ฯ|‹|›|«|»|‘|’|“|”|‱|‰|±|∓|¶|‴|§|‖|¦|©|🄯|℗|®|℠|™|]',r' ', line).split())
		sum = 0.0
		d1 = 0.0
		d2 = 0.0
		cosine[filename] = []
		for char in line:
			if(char in query.lower().split()):
				if(filename in words[char]):#words = tf_idf
					sum += (words[char][filename] * tf_idf_query[char])
					d1 += (words[char][filename]**2)
					d2 += (tf_idf_query[char]**2)
				else:
					d2 += (tf_idf_query[char]**2)
			else:
				if(filename in words[char]):
					d1 += (words[char][filename]**2)
		if(d1 * d2 != 0.0):
			cosine[filename].append(sum / (math.sqrt(d1) * math.sqrt(d2)))
	cosine = dict(collections.OrderedDict(sorted(cosine.items(), key = lambda kv: kv[1], reverse = True)))
	file_relevance = list(islice(cosine, 100))
	return tf_idf_query, file_relevance

def compute_relevance_cosine(tf_idf, query, tf_idf_query, file, cosine, currentThread):
	if(len(file) >= 100):
		begin = int(len(file) / 100 *(currentThread - 1))
		end = int((len(file) / 100)*currentThread - 1)
	else:
		begin = int(currentThread - 1)
		end = int(currentThread - 1)
	for filename in range(begin,end + 1):
		sum = 0.0
		d1 = 0.0
		d2 = 0.0
		cosine[file[filename]] = []
		for item in tf_idf:
			if(file[filename] in tf_idf[item]):
				if(item in tf_idf_query):
					sum += (tf_idf[item][file[filename]] * tf_idf_query[item])
					d1 += (tf_idf[item][file[filename]]**2)
					d2 += (tf_idf_query[item]**2)
				else:
					d1 += (tf_idf[item][file[filename]]**2)
			else:
				if(item in tf_idf_query):
					d2 += (tf_idf_query[item]**2)
		if(d1 * d2 != 0.0):
			cosine[file[filename]].append(sum / (math.sqrt(d1) * math.sqrt(d2)))

def finalCosine(query):
	cosine = {}
	f = codecs.open("tf_idf.txt", 'r', 'UTF-8')
	tf_idf = json.load(f)
	f.close()
	tf_idf_query, file = compute_tf_idf_query("./kq/", tf_idf, query)
	threads = []
	if(len(file) >= 100):
		for i in range(1, 101):
			t = threading.Thread(target = compute_relevance_cosine, args = (tf_idf, query, tf_idf_query, file, cosine, i))
			threads.append(t)
	else:
		for i in range(1, len(file) + 1):
			t = threading.Thread(target = compute_relevance_cosine, args = (tf_idf, query, tf_idf_query, file, cosine, i))
			threads.append(t)
	for thread in threads:
		thread.start()
	for thread in threads:
		thread.join()
	cosine = dict(collections.OrderedDict(sorted(cosine.items(), key = lambda kv: kv[1], reverse = True)))
	keys = list(islice(cosine, 20))
	sorted_cosine = {}
	for key in keys:
		sorted_cosine[key] = cosine[key]
	return sorted_cosine

def build_inverted_tf():
    stopwords = getstopwords("vn.txt")
    build_inverted_index("./kq/", stopwords)
    f = codecs.open('inverted_index.txt', 'r', 'utf-8')
    Inverted_Index = json.load(f)
    f.close()
    compute_tf_idf("./kq/", Inverted_Index)