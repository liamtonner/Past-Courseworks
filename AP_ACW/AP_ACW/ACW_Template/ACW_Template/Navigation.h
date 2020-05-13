#pragma once

#include <fstream>
#include <string>
#include <vector>
#include "Node.h"
#include "Arc.h"
#include <map>
//class Node;

class Navigation
{
	std::ofstream _outFile;

	// Add your code here

public:
	Navigation();
	~Navigation();



	bool BuildNetwork(const std::string& fileNamePlaces, const std::string& fileNameLinks);
	std::vector<int> findShortestRoute(std::map<int, Node>& nodes, Node const & source, const Node & end, const std::string & const mode);
	bool ProcessCommand(const std::string& commandString);
	const float getDistance(const Node *node1, const Node *node2) const;
	// Add your code here
	

private:
	std::map<int, Node> m_nodes;
	std::vector<std::pair<int, Arc>> m_arcs;
};