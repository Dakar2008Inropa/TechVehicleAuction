// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.LoginView.SetSingleton")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.LoginView.GetSingleton~TechAuction.LoginView")]
[assembly: SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>", Scope = "type", Target = "~T:TechAuction.Models.User.ValidationAttributes.CPRNumberValidation")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.Utilities.Helper.Vehicle.GetPlaceholderBitmap~Avalonia.Media.Imaging.Bitmap")]
[assembly: SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.SetForSaleViewModel.OnVehicleImageAdded(System.Object,AuctionData.Models.VehicleModels.VehicleImage)")]
[assembly: SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.SetForSaleViewModel.CreateAuction")]
[assembly: SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.Views.AuctionView.YourAuctions_DoubleTapped(System.Object,Avalonia.Input.TappedEventArgs)")]
[assembly: SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.Views.AuctionView.Auctions_DoubleTapped(System.Object,Avalonia.Input.TappedEventArgs)")]
[assembly: SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.UserProfileViewModel.InitializeAsyncData")]
[assembly: SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.UserProfileViewModel.UpdateAuctionBidsAsync")]
[assembly: SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.UserProfileViewModel.UpdateAuctionBids")]
[assembly: SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Pending>", Scope = "member", Target = "~M:TechAuction.ViewModels.AuctionViewModel.InitializeAsync")]
