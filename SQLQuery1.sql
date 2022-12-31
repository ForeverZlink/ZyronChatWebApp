declare @IdsUserPublicWithNotNotificationModels table (IdUserPublic varchar(100))
declare @Counter integer =1


Insert into @IdsUserPublicWithNotNotificationModels Select   UserPublic.IdPublic
		from UserPublic as UserPublic
		left join  Notifications as Nofications
			on UserPublic.IdPublic = Nofications.IdUserCreatorOfNotification
		where Nofications.IdUserCreatorOfNotification is NULL ;

declare @QuantityOfLoops integer = (Select Count(IdUserPublic) from @IdsUserPublicWithNotNotificationModels);

while 1=1
	begin
		if @Counter >@QuantityOfLoops
			begin
				break
			end
		
		insert into Notifications  (Id,IdUserCreatorOfNotification)
		values(LOWER(newid()),
			 (Select top 1 IdUserPublic from @IdsUserPublicWithNotNotificationModels
				) ) ;
	
	With RowToDelete as (
		Select top 1 IdUserPublic from @IdsUserPublicWithNotNotificationModels)
	
	delete from RowToDelete;
	
	
		

	set @Counter +=1;
	end;

Select * from Notifications;

 