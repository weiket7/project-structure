dotnet ef dbcontext scaffold "server=127.0.0.1;uid=root;pwd=GetGoing!;database=demo" Pomelo.EntityFrameworkCore.MySQL -o Models --context DatabaseContext --force

create table `user` (
id int not null primary key,
username varchar(30) not null,
password varchar(30) not null,
login_at datetime null
)

