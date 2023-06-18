using HotelLibrary.Dtos;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using HotelService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportLibrary.Interfaces;
using ReportLibrary.Model;
using ReportService.Controllers;
using System.Text.Json;

namespace UnitTest
{
    [TestFixture]
    public class Tests
    {
        private HotelController _Hotelcontroller;
        private Mock<IHotelService> _HotelServiceMock;


        private ReportController _Reportcontroller;
        private Mock<IReportService> _ReportServiceMock;

        [SetUp]
        public void Setup()
        {
            _HotelServiceMock = new Mock<IHotelService>();
            _Hotelcontroller = new HotelController(_HotelServiceMock.Object);


            _ReportServiceMock = new Mock<IReportService>();
            _Reportcontroller = new ReportController(_ReportServiceMock.Object);
        }

        /// <summary>
        /// Hotel Test Methods
        /// </summary>
        /// <returns></returns>
        #region HotelTest

            [Test]
            public async Task GetHotelList()
            {
                _HotelServiceMock.Setup(service => service.GetHotelList())
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<List<HotelDto>> { Data = new List<HotelDto>() });

                // Act
                var result = await _Hotelcontroller.GetHotelList();

                // Assert
                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<List<HotelDto>>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<List<HotelDto>>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<List<HotelDto>>(responseData.Data);
                var hotelList = responseData.Data as List<HotelDto>;
                Assert.IsTrue(responseData.IsSuccess);
            }

            [Test]
            public async Task GetHotelById()
            {
                _HotelServiceMock.Setup(service => service.GetHotelById(3))
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<HotelDto> { Data = new HotelDto() });

                // Act
                var result = await _Hotelcontroller.GetHotelById(3);

                // Assert
                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<HotelDto>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<HotelDto>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<HotelDto>(responseData.Data);
                var hotelList = responseData.Data as HotelDto;
                Assert.IsTrue(responseData.IsSuccess);
            }

            [Test]
            public async Task GetHotelManager()
            {
                _HotelServiceMock.Setup(service => service.GetHotelManager(1))
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<List<HotelManagerDto>> { Data = new List<HotelManagerDto>() });

                // Act
                var result = await _Hotelcontroller.GetHotelManagers(1);

                // Assert
                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<List<HotelManagerDto>>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<List<HotelManagerDto>>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<List<HotelManagerDto>>(responseData.Data);
                var hotelList = responseData.Data as List<HotelManagerDto>;
                Assert.IsTrue(responseData.IsSuccess);

            }

            [Test]
            public async Task CreateHotel()
            {
                var json = "{\r\n  \"id\": 0,\r\n  \"name\": \"string\",\r\n  \"addressId\": 0,\r\n  \"address\": {\r\n    \"id\": 0,\r\n    \"addressDetailField\": \"string\",\r\n    \"districtId\": 1,\r\n    \"googleLocation\": \"string\"\r\n  },\r\n  \"hotelPoint\": 0,\r\n  \"hotelStatus\": 0,\r\n  \"isDelete\": false,\r\n  \"oneDayPrice\": 0,\r\n  \"roomType\": 0,\r\n  \"hotelImages\": [\r\n    {\r\n      \"id\": 0,\r\n      \"hotel\": 0,\r\n      \"hotels\": null\r\n    }\r\n  ],\r\n  \"hotelFeatures\": [\r\n    {\r\n      \"id\": 0,\r\n      \"name\": \"string\",\r\n      \"hotelId\": 0,\r\n      \"hotels\": null\r\n    }\r\n  ],\r\n  \"hotelContacts\": [\r\n    {\r\n      \"id\": 0,\r\n      \"telephoneNumber\": \"string\",\r\n      \"hotelEmail\": \"string\",\r\n      \"cOntactCreatedDate\": \"2023-06-13T19:41:01.088Z\",\r\n      \"hotelId\": 0\r\n    }\r\n  ],\r\n  \"webSite\": \"string\",\r\n  \"otelInformation\": \"string\",\r\n  \"hotelCreatedDate\": \"2023-06-13T19:41:01.090Z\",\r\n  \"hotelManagers\": [\r\n    {\r\n      \"id\": 0,\r\n      \"name\": \"string\",\r\n      \"surName\": \"string\",\r\n      \"telephoneNumber\": \"string\",\r\n      \"email\": \"string\",\r\n      \"hotelId\": 0,\r\n      \"hotels\": null,\r\n      \"managerCreatedDate\": \"2023-06-13T19:41:01.090Z\"\r\n    }\r\n  ]\r\n}";
                var model = JsonSerializer.Deserialize<HotelDto>(json);
                _HotelServiceMock.Setup(service => service.CreateHotel(model))
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<HotelDto> { Data = new HotelDto() });

                var result = await _Hotelcontroller.CreateHotel(model);

                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<HotelDto>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<HotelDto>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<HotelDto>(responseData.Data);
                var hotelList = responseData.Data as HotelDto;
                Assert.IsTrue(responseData.IsSuccess);

            }

            [Test]
            public async Task CreateHotelManager()
            {
                var json = "{\r\n  \"id\": 0,\r\n  \"telephoneNumber\": \"string\",\r\n  \"hotelEmail\": \"string\",\r\n  \"cOntactCreatedDate\": \"2023-06-17T22:11:53.776Z\",\r\n  \"hotelId\": 1\r\n}";
                var model = JsonSerializer.Deserialize<HotelContactDto>(json);
                _HotelServiceMock.Setup(service => service.AddHotelContact(model))
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<HotelContactDto> { Data = new HotelContactDto() });

                var result = await _Hotelcontroller.CreateHotelContact(model);

                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<HotelContactDto>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<HotelContactDto>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<HotelContactDto>(responseData.Data);
                var hotelList = responseData.Data as HotelContactDto;
                Assert.IsTrue(responseData.IsSuccess);

            }


            [Test]
            public async Task DeleteHotel()
            {
                _HotelServiceMock.Setup(service => service.DeleteHotel(1))
                        .ReturnsAsync(new HotelLibrary.Dtos.ResponseData<bool> { Data = true });

                var result = await _Hotelcontroller.HotelDelete(1);

                Assert.IsInstanceOf<HotelLibrary.Dtos.ResponseData<bool>>(result);
                var responseData = result as HotelLibrary.Dtos.ResponseData<bool>;
                Assert.IsNotNull(responseData);
                Assert.IsInstanceOf<bool>(responseData.Data);
                Assert.IsTrue(responseData.Data);
                Assert.IsTrue(responseData.IsSuccess);


            }

        #endregion

        /// <summary>
        /// Report test Method
        /// </summary>
        /// <returns></returns>
        #region Report

                [Test]
                public async Task GetListReport()
                {
                    _ReportServiceMock.Setup(service => service.GetListReport())
                            .ReturnsAsync(new ReportLibrary.Model.ResponseData<List<Report>> { Data = new List<Report>() });

                    // Act
                    var result = await _Reportcontroller.GetListReport();

                    // Assert
                    Assert.IsInstanceOf<ReportLibrary.Model.ResponseData<List<Report>>>(result);
                    var responseData = result as ReportLibrary.Model.ResponseData<List<Report>>;
                    Assert.IsNotNull(responseData);
                    Assert.IsInstanceOf<List<Report>>(responseData.Data);
                    var hotelList = responseData.Data as List<Report>;
                    Assert.IsTrue(responseData.IsSuccess);
                }

                [Test]
                public async Task GetReportDetail()
                {
                    _ReportServiceMock.Setup(service => service.GetReportDetail(1))
                            .ReturnsAsync(new ReportLibrary.Model.ResponseData<ReportDetail> { Data = new ReportDetail() });

                    var result = await _Reportcontroller.GetReportDetail(1);
                    Assert.IsInstanceOf<ReportLibrary.Model.ResponseData<ReportDetail>>(result);
                    var responseData = result as ReportLibrary.Model.ResponseData<ReportDetail>;
                    Assert.IsNotNull(responseData);
                    Assert.IsInstanceOf<ReportDetail>(responseData.Data);
                    var hotelList = responseData.Data as ReportDetail;
                    Assert.IsTrue(responseData.IsSuccess);
                }

        #endregion

    }
}