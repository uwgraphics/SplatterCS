function  [ X,Y ] = g2( x,y,sx,sy,n,angle)
%G2 Summary of this function goes here
%   Detailed explanation goes here
u1 = rand([n 1]);
u2 = rand([n 1]);
Y = sqrt(-2*log(u1)).*sin(2*pi*u2);
X = sqrt(-2*log(u1)).*cos(2*pi*u2);

Y = Y.*sx;
X = X.*sy;
rad = angle*(pi/180);
rot = [ cos(rad) -sin(rad);
        sin(rad) cos(rad)];
XY = [X';Y'];
XY = rot*XY;
X = XY(1,:)' + x;
Y = XY(2,:)' + y;
end