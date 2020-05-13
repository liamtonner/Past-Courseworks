#include "Node.h"


using namespace std;

Node::Node()
{
	Lattd = 0;
	Longtd = 0;
	Ref = 0;
}

void Node::buildNode(const string & placename, int const ref, float const longtd, float const lattd) {
	placeName = placename;
	Ref = ref;
	Longtd = longtd;
	Lattd = lattd;
}

void Node::addLink(Arc* const arc)
{
		m_arcs.push_back(arc);
}

int const Node::GetRef() const
{
	return Ref;
}

std::string Node::GetName() const
{
	return placeName;
}

float Node::GetLong() const
{
	return Longtd;
}

float Node::GetLatt() const
{
	return Lattd;
}

bool Node::arcCheck(const int ID, const string & mode)
{
	for (int i = 0; i < m_arcs.size(); i++) {
		Arc const temp = *m_arcs[i];
		if (temp.GetDestination()->Ref == ID && temp.routeCheck(mode)) {
			return true; // passes
		}
	}
	return false; //failed
}
std::vector<Arc*> Node::GetArcs() const
{
	return m_arcs;
}




