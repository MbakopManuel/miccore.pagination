# Miccore .Net Pagination
Require .Net core 6.0

Package library for pagination when using query and entity framework core

# Intallation

dotnet CLI installation
```sh

dotnet add package Miccore.Net.Pagination

```
Package manager
```sh

Install-Package Miccore.Net.Pagination

```

In the following points, we will take an example of adding paging to a list of notifications in the server.

We use here an architecture based on the service-repository pattern. So we'll start from the repositories to get to the controller

## Using in repository

Include all namespaces
```csharp
  using Miccore.Pagination.Models;
  using Microsoft.Extensions.DependencyInjection;
  ```

Add logic to the function
```csharp
...
public  class  NotificationRepository : INotificationRepository {
	...
	public  async  Task<PaginationModel<NotificationDtoModel>> GetAllAsync(PaginationQuery query)
	{
		var  notifications  =  await  _context.Notifications.PaginateAsync(query);
		return  notifications;
	}
	...
}
  ```

```csharp
public  interface  INotificationRepository{
...
Task<PaginationModel<NotificationDtoModel>> GetAllAsync(PaginationQuery query);
...
}
  ```

## Using in Service

We are using here a service mapper like AutoMapper
first of all we have to add Mapper Profile.
 > if you don't use it, the next code is not for you
 
 Include models
 ```csharp
  using Miccore.Pagination.Models;
  ```
  set Profile
 ```csharp
 
...
public  class  NotificationProfile : Profile
{
	...
	public  NotificationProfile()
	{
		CreateMap<PaginationModel<NotificationDomainModel>, PaginationModel<NotificationDtoModel>>().ReverseMap();
	}
	...
}
  ```

After setting profile or not, you have now to set the service and interface
```csharp
...
public  class  NotificationService : INotificationService {
	...
	public  async  Task<PaginationModel<NotificationDtoModel>> GetAllNotificationAsync(PaginationQuery query)
	{
		var  notifications  =  await  _notificationRepository.GetAllAsync(query);
		return  _mapper.Map<PaginationModel<NotificationDomainModel>>(notifications);
	}
	...
}
  ```

```csharp
public  interface  INotificationService{
...
Task<PaginationModel<NotificationDtoModel>> GetAllNotificationAsync(PaginationQuery query);
...
}
  ```
## Using in controller

> like in the service, if you are using AutoMapper, you have to set the profile of mapper before map element in the function

Include elements in controller
 ```csharp
  using Miccore.Pagination.Models;
  using Miccore.Pagination.Service;
  ```
Update controller function
```csharp
...
public  class  NotificationController : BaseController {
	...
	[HttpGet(template: "", Name =  nameof(GetAllNotification))]
	public  async  Task<ActionResult<PaginationModel<NotificationViewModel>>> GetAllNotification([FromQuery] PaginationQuery query)
	{
		try
		{
			var  notifications  =  await  _notificationService.GetAllNotificationsAsync(query);
			var  response  =  _mapper.Map<PaginationModel<NotificationViewModel>>(notifications);
			// return only items if paginate commande is false
			if(!query.paginate){
				return  HandleSuccessResponse(response.Items);
			}
			// add the previous and next url of pages if exists
			response.AddRouteLink(Url.RouteUrl(nameof(GetAllNotification)), query);
			return  HandleSuccessResponse(response);
		}
		catch (Exception  ex)
		{
			return  HandleErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
		}
	}
	...
}
  ```

🥳 🤩
**Well done, you can now test your feature**


### Models content

Pagination Model definition
 ```csharp
public  class  PaginationModel<TModel>{
	const  int MaxPageSize =  100;
	private  int _pageSize;
	public  int PageSize {
		get  =>  _pageSize;
		set  =>  _pageSize  = ( value  >  MaxPageSize) ?  MaxPageSize  :  value;
	}
	public  int CurrentPage {get; set;}
	public  int TotalItems {get; set;}
	public  int TotalPages { get; set;}
	public  List<TModel> Items {get; set;}
	public  string Prev {get; set;}
	public  string Next {get; set;}
	public  PaginationModel(){
		Items  =  new  List<TModel>();
	}
}
  ```

Pagination Query
 ```csharp
public  class  PaginationQuery{
	[DefaultValue(false)]
	public  bool paginate { get; set;}
	[DefaultValue(1)]
	public  int page { get; set;}
	[DefaultValue(10)]
	public  int limit { get; set;}
}
  ```

> paginate says whether the returned content should be paginated or not
