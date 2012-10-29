Groups = [1 2 3 4 5 6 7 8];
NumPoints = [1000 10000 50000 100000 500000 1000000 2000000 3000000];

for i = 1:length(Groups)
    for j = 1:length(NumPoints)
        GenData(NumPoints(j),Groups(i));
    end
end
