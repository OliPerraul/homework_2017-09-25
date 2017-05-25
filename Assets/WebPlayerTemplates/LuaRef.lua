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

start = function(self)
end,

update = function(self) 
self.x = self.x + .5;
self.y = sin(self.x);
end,

}

b = new(Bullet);

add(b);





