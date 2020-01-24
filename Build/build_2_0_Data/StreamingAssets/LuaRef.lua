function new(Entity)
	instance = {}

	for key, value in pairs(Entity) do
	instance[k] = value;
	end

	return instance;
end

--no sin
Bullet = 
{
x =0,
y=0,

start = function(self)
end,

update = function(self) 
self.x = self.x + 1;
end,

}

b = new(Bullet);

add(b);

--sin
Bullet = 
{
x =0,
y=0,
spd =.3,


start = function(self)
end,

update = function(self) 
self.x = self.x + self.spd;
self.y = sin(self.x);
end,

}

b = new(Bullet);

add(b);


--Wait and shoot
Bullet = 
{
x =0,
y=0,
spd =0,
timer=0,

start = function(self)
end,

update = function(self) 

if self.timer> 200 then
self.spd = .5;
end
self.timer = self.timer+1;

self.x = self.x + self.spd;
end,

}

b = new(Bullet);

add(b);


--Small sin
Bullet = 
{
x =0,
y=0,
spd =0,

start = function(self)
end,


update = function(self) 

self.spd =self.spd+.01;
self.x = self.x+self.spd;

self.y = sin(self.spda)*sin(self.x);

end

}

b = new(Bullet);

add(b);



--Orbitting
Bullet = 
{
sprite = spr_new;
x =0,
y=0,

a =0,

start = function(self)
end,


update = function(self) 

self.a = self.a+ 0.1;

self.x  =  cos(self.a)*getplayerx()-sin(self.a)*(getplayery());
self.y  =  sin(self.a)*getplayerx()+cos(self.a)*(getplayery());
end

}

b = new(Bullet);

add(b);


--Small sin
Bullet = 
{
sprite = spr_new;
x =getplayerx(),
y=getplayery()+5,

a =0,

start = function(self)
end,


update = function(self) 

startx = getplayerx();
starty = getplayery()+5;

self.a = self.a+ 0.01;

self.x  =  cos(self.a)*(startx-getplayerx())-sin(self.a)*(starty-getplayery())+getplayerx();
self.y  = sin(self.a)*(startx-getplayerx())+cos(self.a)*(starty-getplayery())+getplayery();
end

}

b = new(Bullet);

add(b);


---REGULAR

--Small sin
Bullet = 
{
sprite = spr_new,
type = "bullet",
x =getmousex(),
y=getmousey(),
spd =0,

start = function(self)
end,


update = function(self) 

self.spd =self.spd+.01;
self.x = self.x+self.spd;
end

}

for i = 0, .5, .1 do
b = new(Bullet);
b.y = b.y+i;
add(b);
end

b = new(Bullet);

add(b);



obj_bullet=
{
type = "bullet",
sprite = spr_new,
x = getplayerx(),
y = getplayery()+5,

-- Start 
start = function(self)
end,

-- Update
update = function(self)
self.x = self.x+.5;
end

}

