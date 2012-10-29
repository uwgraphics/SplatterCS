function [ X,Y ] = g( x,y,s,n)
%G Summary of this function goes here
%   Detailed explanation goes here
u1 = rand([n 1]);
u2 = rand([n 1]);
Y = sqrt(-2*log(u1)).*sin(2*pi*u2);
X = sqrt(-2*log(u1)).*cos(2*pi*u2);

Y = Y.*s + y;
X = X.*s + x;
end