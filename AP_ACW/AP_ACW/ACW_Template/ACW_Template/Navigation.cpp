#include "Navigation.h"
#include <iostream>
#include <iomanip>
#include <sstream>
#include <vector>

using namespace std;

// Converts latitude/longitude into eastings/northings
extern void LLtoUTM(const double Lat, const double Long, double &UTMNorthing, double &UTMEasting);


Navigation::Navigation() : _outFile("Output.txt")
{
}

Navigation::~Navigation()
{
}

bool Navigation::ProcessCommand(const string& commandString)
{

	int count = 0;
	istringstream inString(commandString);
	string command;
	inString >> command;

	_outFile.setf(ios::fixed, ios::floatfield);

	_outFile.precision(3);

	float maxDist = 0;
	string name1, name2;

	if (strcmp(command.c_str(), "MaxDist") == 0) 
	{
		for (auto i = m_nodes.begin(); i != m_nodes.end(); i++) {
			for (auto a = i; a != m_nodes.end(); a++) {
				Node const node1 = i->second;
				Node const node2 = a->second;
				if (node1.GetRef() != node2.GetRef()) {
					float const dist = getDistance(&node1, &node2);

					if (dist > maxDist) {
						maxDist = dist;
						name1 = i->second.GetName();
						name2 = a->second.GetName();
					}
				}
			}
		}
		_outFile << "MaxDist \n" << name1 << "," << name2 << "," << maxDist << endl << endl;
		return true;
	}
	else if (strcmp(command.c_str(), "MaxLink") == 0) 
	{
		float maxDist = 0;
		int id1, id2;
		for (int i = 0; i < m_arcs.size(); i++) {
			Node const node1 = m_nodes.at(m_arcs[i].first);
			Node* const node2 = m_arcs[i].second.GetDestination();

			float const dist = getDistance(&node1, node2);
			if (dist > maxDist) {
				maxDist = dist;
				id1 = node1.GetRef();
				id2 = node2->GetRef();
			}
		}
		_outFile << "MaxLink \n" << id1 << "," << id2 << "," << maxDist << endl << endl;
		return true;
	}
	else if (strcmp(command.c_str(), "FindDist") == 0) 
	{
		int node1id, node2id;
		inString >> node1id;
		inString >> node2id;
		string const node1 = m_nodes.at(node1id).GetName();
		string const node2 = m_nodes.at(node2id).GetName();
		float const dist = getDistance(&m_nodes.at(node1id), &m_nodes.at(node2id));

		_outFile << "FindDist " << node1id << " " << node2id << endl << node1 << "," << node2 << "," << dist << endl << endl;
		return true;
	}
	else if (strcmp(command.c_str(), "FindNeighbour") == 0) 
	{
		int node1id;
		inString >> node1id;
		Node const node1 = m_nodes.at(node1id);
		_outFile << "FindNeighbour" << " "  << node1id << endl;
		std::vector<Arc*> hold = m_nodes.at(node1id).GetArcs();
		for (int i = 0; i < hold.size(); i++)
		{
			_outFile << hold[i]->GetDestination()->GetRef()<< endl;
		}
		_outFile << endl;
		return true;
	}
	else if (strcmp(command.c_str(), "Check") == 0) 
	{
		string transportMethod;
		int X;
		std::vector<int> route;
		inString >> transportMethod;

		_outFile << "Check" << " " << transportMethod << " ";

		while (inString.good()) {
			inString >> X;
			_outFile << X << " ";
			route.push_back(X);
		}
		Node node1 = m_nodes.begin()->second;
		_outFile << endl;
		for (int i = 0; i < route.size() - 1; i++) 
		{
			node1 = m_nodes.at(route[i]);
			count++;
			if (node1.arcCheck(route[i + 1], transportMethod)) 
			{
				_outFile << route[i] << "," << route[i + 1] << "," << "PASS" << endl;
			}
			else
			{
				_outFile << route[i] << "," << route[i + 1] << "," << "FAIL" << endl;
				break;
			}
		}
		_outFile << endl;
		return true;
	}

	else if (strcmp(command.c_str(), "FindRoute") == 0)
	{
		std::string mode, startIDstring, endIDstring;
		int startID, endID;

		inString >> mode;
		inString >> startIDstring;
		inString >> endIDstring;

		std::istringstream iss(startIDstring);
		iss >> startID;
		iss = std::istringstream(endIDstring);
		iss >> endID;

		_outFile << "FindShortestRoute " << mode << " " << startID << " " << endID << endl;

		std::vector<int> holder = findShortestRoute(m_nodes, m_nodes.at(startID), m_nodes.at(endID), mode);

		for (int i = 0; i < holder.size(); i++)
		{
			_outFile << holder[i] << endl;
		}
		_outFile << endl;
		return true;
	}
	else if (strcmp(command.c_str(), "FindShortestRoute") == 0)
	{
		std::string mode, startIDstring, endIDstring;
		int startID, endID;

		inString >> mode;
		inString >> startIDstring;
		inString >> endIDstring;

		std::istringstream iss(startIDstring);
		iss >> startID;
		iss = std::istringstream(endIDstring);
		iss >> endID;

		_outFile << "FindShortestRoute " << mode << " " << startID << " " << endID << endl;

		std::vector<int> holder = findShortestRoute(m_nodes, m_nodes.at(startID), m_nodes.at(endID), mode);

		for (int i = 0; i < holder.size(); i++)
		{
			_outFile << holder[i] << endl;
		}
		_outFile << endl;
		return true;
	}
	
	return false;
}

const float Navigation::getDistance(const Node * const node1 ,const Node * const node2 ) const
{
	double x1, x2, y1, y2;

	LLtoUTM(node1->GetLatt(), node1->GetLong(), y1, x1);
	LLtoUTM(node2->GetLatt(), node2->GetLong(), y2, x2);

	return sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)))/1000;
}

bool Navigation::BuildNetwork(const string &fileNamePlaces, const string &fileNameLinks)
{
	fstream finPlaces(fileNamePlaces);
	fstream finLinks(fileNameLinks);
	if (finPlaces.fail() || finLinks.fail()) return false;
	while (finPlaces.good())
	{
		char line[255];
		string placename;
		int ref;
		float longtd, lattd;

		finPlaces.getline(line, 255, ',');
		placename = string(line);

		finPlaces.getline(line, 255, ',');
		istringstream sin(line);
		sin >> ref;

		finPlaces.getline(line, 255, ',');
		istringstream sin2(line);
		sin2 >> lattd;

		finPlaces.getline(line, 255, '\n');
		istringstream sin3(line);
		sin3 >> longtd;
		Node node;

		node.buildNode(placename, ref, longtd, lattd);

		m_nodes.insert({ ref, node });

	}
		while (finLinks.good())
		{
			std::string string, transportMode, newString;
			int i = 0, ref1, ref2;
			std::getline(finLinks, string);

			std::stringstream ss(string);

			while (std::getline(ss, newString, ',')) 
			{
				switch (i) 
				{
				case 0:
					ref1 = stoi(newString);
					break;
				case 1:
					ref2 = stoi(newString);
					break;
				case 2: 
					transportMode = newString;
				}
				i++;
			}

			Node const node;

			Arc arc1;

			Arc arc2;

			arc1.buildArc(&m_nodes.find(ref2)->second, transportMode);

			arc2.buildArc(&m_nodes.find(ref1)->second, transportMode);

			m_arcs.push_back({ ref1, arc1 });

			m_arcs.push_back({ ref2, arc2 });			

		}
		for (int i = 0; i < m_arcs.size(); i++) {
			m_nodes.at(m_arcs[i].first).addLink(&m_arcs[i].second);
		}

		return true;
	
}
std::vector<int> Navigation::findShortestRoute(std::map<int, Node>& nodes, Node const &source, const Node& end, const std::string& const mode)
{
	int count = 0;
	std::map<int, float> distance;
	std::map<int, Node> Q;
	std::vector<int> IDRoute;


	for (auto v = nodes.begin(); v != nodes.end(); v++)
	{
		distance.insert({ v->second.GetRef(), 100000 });
		Q.insert({ v->second.GetRef(), v->second });
		count++;
	}
	distance.at(source.GetRef()) = 0;

	while (Q.size() != 0)
	{
		float lowestValue = 10000000;
		int lowestValueID = 0;
		for (auto i = distance.begin(); i != distance.end(); i++)
		{
			if (i->second < lowestValue)
			{
				std::map<int, Node>::iterator it;
				it = Q.find(i->first);
				if (it != Q.end())
				{
					lowestValue = i->second;
					lowestValueID = i->first;
				}
			}
		}

		Q.erase(lowestValueID);
		std::vector<Arc*> holder = nodes.at(lowestValueID).GetArcs();
		for (auto u = holder.begin(); u != holder.end(); u++)
		{
			const Arc pointer = **u;
			float alt;
			alt = distance.at(lowestValueID) + getDistance(&nodes.at(lowestValueID), pointer.GetDestination());

			if (alt < distance.at(pointer.GetDestination()->GetRef()))
			{
				distance.at(pointer.GetDestination()->GetRef()) = alt;
			}
		}
		count++;
	}

	float enddistance;
	enddistance = distance.at(end.GetRef());
	Node current = end;
	bool startFound;
	startFound = false;

	IDRoute.push_back(current.GetRef());

	while (!startFound)
	{
		int counter;
		counter = 0;
		float lowestvalue = 1000000;
		int ID = 0;
		std::vector<Arc*> holder = current.GetArcs();
		for (auto i = holder.begin(); i != holder.end(); i++)
		{
			Arc const pointer = **i;
			if (distance.at(pointer.GetDestination()->GetRef()) < lowestvalue && pointer.routeCheck(mode))
			{
				if (std::find(IDRoute.begin(), IDRoute.end(), pointer.GetDestination()->GetRef()) != IDRoute.end())
				{
					_outFile << "FAIL" << endl;
					const std::vector<int> empty;
					IDRoute = empty;
					return IDRoute;
				}

				ID = pointer.GetDestination()->GetRef();
				lowestvalue = distance.at(pointer.GetDestination()->GetRef());
			}
			count++;
		}
		IDRoute.push_back(ID);
		current = nodes.at(ID);
		int checker;
		checker = distance.at(ID);
		if (checker == 0)
			break;
	}
	std::reverse(IDRoute.begin(), IDRoute.end());
	return IDRoute;
}

// Add your code here


