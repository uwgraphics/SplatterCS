% test case 1
n = 5000;
m = 50000;
[X0,Y0] = g(0,0,1/4,n);
[X1,Y1] = g(0,0,5,m);
[X2,Y2] = g(-2,-2,1/4,n);
[X3,Y3] = g(-4,-4,1/4,n);
[X4,Y4] = g(2.5,2.5,1/4,n);
[X5,Y5] = g(3.5,3.5,1/4,n);
X = [X0; X1; X2; X3;X4;X5];
Y = [Y0; Y1; Y2; Y3;Y4;Y5];
XY = [X Y];
[x0,y0] = g(3,3,1,m);
% [x1,y1] = g(-1,1,1/4,n);
% [x2,y2] = g(-1,-1,1/3,n);
% [x3,y3] = g(1,-1,1/2,n);
x = [x0];
y = [y0];
xy=[x0 y0];
figure(1);
hold on    
plot(X,Y,'.',x,y,'.')
hold off
axis equal
Write(XY,{'0','1'},'test2.txt');
Write(xy,{'0','1'},'test3.txt');