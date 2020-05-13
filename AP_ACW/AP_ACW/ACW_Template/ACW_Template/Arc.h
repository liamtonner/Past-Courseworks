// Arc.h
#pragma once
#include <string>
using namespace std;
const enum travel
{
	Foot, Bike, Car, Bus, Rail, Ship
};
class Node;

class Arc
{

public:
	Arc();
	void buildArc(Node* node , const string &transportMode);
	Node * GetDestination() const;
	string GetTransportMode() const;
	bool routeCheck(std::string const &mode)const;
	travel changeToEnum(std::string const &transportMode) const;


private:
	Node * m_destination = nullptr;
	string TransportMode;

}; 
 