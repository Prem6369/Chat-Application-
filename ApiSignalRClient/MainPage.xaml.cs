using Microsoft.AspNetCore.SignalR.Client;

namespace ApiSignalRClient
{
    public partial class MainPage : ContentPage
    {
        private readonly HubConnection hubConnection;
        private string username;
        public MainPage()
        {
            InitializeComponent();

            var baseUrl = "http://localhost";

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }

            hubConnection = new HubConnectionBuilder()
               .WithUrl($"{baseUrl}:5279/ChatHub")
               .Build();


            hubConnection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    Label lbltext = new Label
                    {
                        Text = $"{user} says: {message}"
                    };
                    if(user == username)
                    {
                        lbltext.HorizontalOptions = LayoutOptions.End;
                    }
                    else
                    {
                        lbltext.HorizontalOptions = LayoutOptions.Start;
                    }

                    lblChat.Children.Add(lbltext);
                });
            });

            Task.Run(async () =>
            {
                try
                {
                    await hubConnection.StartAsync();

                    string connectionId = await hubConnection.InvokeCoreAsync<string>("GetConnectionId", Array.Empty<object>());

                    // Update UI with connection ID
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        conncetionid.Text = $"Connection ID: {connectionId}";
                    });

                }
                catch (Exception ex)
                {
                    // Handle connection start errors
                    Console.WriteLine($"Error starting SignalR connection: {ex}");
                }
            });



        }

        private async void Sendbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtRecevierid == null)
                {
                    username = txtUsername.Text;
                    await hubConnection.InvokeCoreAsync("SendMessageToAll", args: new[]
                         { txtUsername.Text, txtMessage.Text });
                }
                else
                {
                    username = txtUsername.Text;
                    await hubConnection.InvokeCoreAsync("SendMessageUser",
                        args: new[] { txtUsername.Text, txtRecevierid.Text, txtMessage.Text });
                }                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex}");
                Console.WriteLine(ex.ToString());
            }

        }

        

    }

}
