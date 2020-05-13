#include "Arc.h"
Arc::Arc()
{
	m_destination = nullptr;
	TransportMode;
}

void Arc::buildArc( Node * const node , const string &transportMode)
{
	m_destination = node;
	TransportMode = transportMode;
}

Node* Arc::GetDestination() const
{
	return m_destination;
}

string  Arc::GetTransportMode() const
{
	return TransportMode;
}

bool Arc::routeCheck(std::string const & mode) const
{
	travel t1, t2;
	t1 = changeToEnum(TransportMode);
	t2 = changeToEnum(mode);
	switch (t1) 
	{
	case travel::Foot:
		return true;
		break;
	case Bike:
		if (t2 >= 1) 
			return true;
		
		else 
			return false;
		break;
	case Car:
		if (t2 == 2 || t2 == 3 || t2 == 5)
			return true;
		else
			return false;
		break;
	case Bus:
		if (t2 == 3 || t2 == 5)
			return true;
		else
			return false;
		break;
	case Rail:
		if (t2 == 4)
			return true;
		else
			return false;
		break;
	case Ship:
		if (t2 == 5)
			return true;
		else
			return false;
		break;
	default:
		t1 == t1;
		break;

	}
	return false;
}

travel Arc:: changeToEnum(std::string const & transportMode) const
{
	travel travel = Foot;
	if (strcmp(transportMode.c_str(), "Foot") == 0){
		travel = Foot;
	}
	else if (strcmp(transportMode.c_str(), "Bike") == 0) {
		travel = Bike;
	}
	else if (strcmp(transportMode.c_str(), "Car") == 0) {
		travel = Car;
	}
	else if (strcmp(transportMode.c_str(), "Bus") == 0) {
		travel = Bus;
	}
	else if (strcmp(transportMode.c_str(), "Rail") == 0) {
		travel = Rail;
	}
	else if (strcmp(transportMode.c_str(), "Ship") == 0) {
		travel = Ship;
	}
	return travel;
}




