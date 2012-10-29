function  GenData( pointNum, groupNum )
%GENDATA Summary of this function goes here
%   Detailed explanation goes here


n = round(pointNum/groupNum);

index = 0;
X = [];
Y = [];
G = [];
step = (2*pi)/groupNum;
for i = 1:groupNum
    angle = (i-1)*step;
    rad = 5;
    x = cos(angle)*rad;
    y = sin(angle)*rad;
    [x0,y0] = g(x,y,3,n);
    g0 = zeros(size(x0)) + index;
    index = index+1;
    X = [X;x0];
    Y = [Y;y0];
    G = [G;g0];
end
XY = [X Y G];
%plot(XY(:,1),XY(:,2),'.')
Write(XY,{'0','1','G'},['perf_' int2str(groupNum)  '_' int2str(pointNum) '.txt']);

end

