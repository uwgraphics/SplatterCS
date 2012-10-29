% test case 1
clear;
n = 5000;
[X0,Y0] = g2(0+1,0,1,1,n,45);
[X1,Y1] = g2(2,1.5,.5,1,n/2,0);
[X2,Y2] = g2(0-1,3,.75,.75,n/2,-45);
[X3,Y3] = g2(2.5-1,2.0,.5,.5,n,-45);

g0 = zeros(size(X0));
g1 = zeros(size(X1));
g2 = zeros(size(X2));
g3 = zeros(size(X3))+1;

X = [X0; X1; X2; X3;];
Y = [Y0; Y1; Y2; Y3;];
G = [g0; g1; g2; g3;];
XY = [X Y G];

figure(1);
hold on    
plot(X0,Y0,'.',X1,Y1,'.',X2,Y2,'.',X3,Y3,'.')
hold off
axis equal

Write(XY,{'0','1','G'},'diagram.txt');