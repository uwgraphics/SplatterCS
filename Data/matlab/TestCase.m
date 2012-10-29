% test case 1
[X0,Y0] = g(1,1,1/3);
[X1,Y1] = g(-1,1,1/2);
[X2,Y2] = g(-1,-1,1/3);
[X3,Y3] = g(1,-1,1/4);
X = [X0; X1; X2; X3];
Y = [Y0; Y1; Y2; Y3];
XY = [X Y];
[x0,y0] = g(1,1,1/3);
[x1,y1] = g(-1,1,1/4);
[x2,y2] = g(-1,-1,1/3);
[x3,y3] = g(1,-1,1/2);
x = [x0; x1; x2; x3];
y = [y0; y1; y2; y3];
xy=[x y];
figure(1);
hold on
num = size(X,1);
i=1;
j=1;
% while i<=num &&j<=num
%     plot(X(i),Y(i),'.',x(i),y(i),'.');
%     i=i+1;
%     j=j+1;
% end
    
plot(X,Y,'.',x,y,'.')
hold off
axis equal
tic
blue = dataset({XY 'dim1' 'dim2'});
export(blue,'file','blue.txt','Delimiter',',');
toc
green = dataset({xy 'dim1' 'dim2'});
export(green,'file','green.txt','Delimiter',',');

tic
Write(XY,{'dim1','dim2'},'blue2.txt');
toc
