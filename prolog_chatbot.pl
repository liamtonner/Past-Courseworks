agent:-
    write('Enter Your Sentence In List Form: '),
    perceive(Percepts),
    action(Percepts).

perceive(Percepts) :-
    read(Percepts).

action(Percepts) :-
    sentence(Percepts, Output),
    unify(Percepts,Output),
    nl,
    nl,
    /*
    Output = sentence(A,B),
    nl,
    nl,
    unify(A),
    unify(B),
    nl,
    */
    ptree(Output).

sentence(Sentence,sentence((Noun_Phrase),(Verb_Phrase))):-
/* so take a sentence (first arg) and
parse it into a noun phrase and a verb phrase */
    np(Sentence,Noun_Phrase,Rem),
    vp(Rem,Verb_Phrase).

np([X|T],np(det(X),NP2),Rem):-
    det(X),
    np2(T,NP2,Rem).
np(Sentence,Parse,Rem):- np2(Sentence,Parse,Rem).
np(Sentence,np(NP,PP),Rem):-
    /* e.g. Jane on the dance_floor */
    np(Sentence,NP,Rem1),
    pp(Rem1,PP,Rem).

np2([H|T],np2(noun(H)),T):- noun(H).
/* ok cute H, so you are a noun */
np2([H|T],np2(adj(H),Rest),Rem):- adj(H),np2(T,Rest,Rem).
/* shove the adj(H) into the to be retured answer and
then recurse on the rest of the phrase to return the
parse and the remainder of the sentence */

pp([H|T],pp(prep(H),Parse),Rem):-
    prep(H),
    np(T,Parse,Rem).

vp([H|[T]],vp(verb(H),T)):-
    verb(H).
vp([H|Rest],vp(verb(H),RestParsed)):-
    verb(H),
    np(Rest, RestParsed, _).
vp([H|Rest],vp(verb(H),RestParsed)):-
    verb(H),
    pp(Rest,RestParsed,_).

put_if(S, Cond) :-
    Cond,
    S.
put_if(_, Cond) :-
    not(Cond).

pargs([], _).
pargs([H|T], N) :-
    ptree(H, N),
    put_if(nl, T \== []),
    pargs(T, N).

ptree(E, N) :-
    E =.. [H|T],
    length(T, Len),
    put_if(tab(N), Len >= 1),
    write(H),
    put_if(write('|-'), Len >= 1),
    put_if(nl, Len >= 2),    Nx is N + 2,
    pargs(T, Nx).

ptree(E) :-
    ptree(E, 0).

/*
unify(np(A,B)) :-
    print(A),
    unify(B).

unify(np2(A,B)) :-
    print(A),
    unify(B).

unify(np2(B)) :-
    print(B).

unify(vp(A,B)) :-
    print(A),
    unify(B).

unify(pp(A,B)) :-
    print(A),
    unify(B).
*/


unify(Percepts,Input):-
    write(Input),
    nl,
	Input = sentence(NounPhrase,VerbPhrase),
        NounPhrase = np(A,B),
        B = np2(C,D),
        D = np2(E),

                        nl,
                VerbPhrase = vp(Z,Y),
                Y = np(X,W),
                 W = np2(V,U),
                 U = np2(T),
                 T = noun(ParseNoun),
                 Z = verb(ParseVerb),
                 recommend(ParseVerb,ParseNoun, RecommendPhrase),
                 write('|'),
                 tab(1),
                 write('|-'),
                 print(Percepts),
                 nl,
                 nl,
                 write('|'),
                 tab(1),
                 write('|-'),
                 write('produces the recommendation that'),
                 nl,
                 nl,
                 tab(2),
                 write('|'),
                 tab(1),
                 write('|-'),
                 print(RecommendPhrase).
det(an).
det(a).
det(the).

adj(good).
adj(old).
adj(teenage).
adj(sprightly).
adj(long).
adj(young).
adj(social).
adj(avid).
adj(big).
adj(fat).
adj(racing).

noun(father).
noun(book).
noun(boy).
noun(horses).
noun(grandfather).
noun(person).
noun(student).
noun(petrolhead).
noun(car).
noun(cat).
noun(walk).
noun(chat).

verb(likes).
verb(loves).

recommend(likes,book,'He joins the book club.').
recommend(loves,horses,'They join a riding club').
recommend(loves,walk,'He joins a rambling club').
recommend(likes,chat,'They join a social club').
recommend(likes,guitar,'They should join a band').
recommend(loves,racing,'They should go to the races').


