% test case 1

[X,Y] = grand();
XY = [X Y];
[x,y] = grand();
xy=[x y];
[xx,yy] = grand();
xxyy=[xx yy];

figure(1);
hold on    
plot(X,Y,'.',x,y,'.',xx,yy,'.')
hold off
axis equal
Write(XY,{'0','1'},'grand2.txt');
Write(xy,{'0','1'},'grand3.txt');
Write(xxyy,{'0','1'},'grand4.txt');