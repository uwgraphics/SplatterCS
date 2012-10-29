% test case 1
clear;
n = 50000;
[X0,Y0] = g2(0+3,0,1,3,n,45);
[X1,Y1] = g2(-1.5+3,1.5,1,3,n,45);
[X2,Y2] = g2(0-3,0,1,3,n,-45);
[X3,Y3] = g2(1.5-3,1.5,1,3,n,-45);

g0 = zeros(size(X0));
g1 = zeros(size(X1))+1;
g2 = zeros(size(X2))+2;
g3 = zeros(size(X3))+3;

X = [X0; X1; X2; X3;];
Y = [Y0; Y1; Y2; Y3;];
G = [g0; g1; g2; g3;];
XY = [X Y G];

figure(1);
hold on    
plot(X0,Y0,'.',X1,Y1,'.',X2,Y2,'.',X3,Y3,'.')
hold off
axis equal

Write(XY,{'0','1','G'},'venn4.txt');