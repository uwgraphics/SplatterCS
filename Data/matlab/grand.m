function [ X,Y ] = grand( )
%GRAND Summary of this function goes here
%   Detailed explanation goes here
centers = int32(rand(1)*5+5);
X = 0;
Y = 0;
for i=1:centers
    cx = rand(1)*10 - 5;
    cy = rand(1)*10 - 5;
    s = rand(1)*2;
    n = int32(rand(1)*3000 + 5000);
    [X0,Y0] = g(cx,cy,s,n);
    X = [X;X0];
    Y = [Y;Y0];
end

end

