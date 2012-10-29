function  Write( data,columnames,filename)
%WRITE Summary of this function goes here
%   Detailed explanation goes here
fid = fopen(filename,'w');
fprintf(fid,'%s,',columnames{1:end-1});
fprintf(fid,'%s\n',columnames{end});
fprintf(fid, [repmat('%.15g,',1,size(data,2)-1) '%.15g\n'], data');  %'# default prec=15
fclose(fid);
end

