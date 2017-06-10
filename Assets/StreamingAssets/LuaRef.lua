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

self.y = sin(self.spd)*sin(self.x);

end

}

b = new(Bullet);

add(b);



