// Node.h
#pragma once
#include <string>
#include <vector>
#include "Arc.h"
using namespace std;

class Node
{
public:
	Node();
	void buildNode(const string &placename, int ref, float longtd, float lattd);
	void addLink(Arc* arc);
	int const GetRef() const;
	std::string GetName() const;
	float GetLong() const;
	float GetLatt() const;
	bool arcCheck(int ID, const string &mode);
	std::vector<Arc*> GetArcs() const;


private:
	string placeName;
	int Ref;
	float Longtd, Lattd;
	vector<Arc*> m_arcs;

}; 

