using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using metricstddlib;

namespace metricstddtest
{
    public class WidgetTest
    {
        [Test]
        public void ShouldHaveSelectedFields()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache())
                .Returns(new CachedNumber());
            var widget = new Widget(service.Object);
            widget.Init();

            widget.AddField("URL", string.Empty);
            widget.AddField("Options", "Likes", "People talking about");

            var fields = widget.GetFields();
            Assert.AreEqual(2, fields.Count);
        }
        
        [Test]
        public void ShouldOnlyCallCache()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache())
                .Returns(new CachedNumber());
            var widget = new Widget(service.Object);
            widget.Init();
            service.Verify(s => s.GetNumberFromCache());
            service.Verify(s => s.GetNumber(), Times.Never);
        }

        [Test]
        public void ShouldCallCacheBeforeUpdating()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache())
                .Returns(new CachedNumber());
            var widget = new Widget(service.Object);
            widget.Init();
            service.Verify(s => s.GetNumberFromCache());
            widget.Update();
            service.Verify(s => s.GetNumber());
        }

        [Test]
        public void UpdateTimeShouldBeUpdatedIfGetNumberIsSuccesful()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache())
                .Returns(new CachedNumber() { Value = 10, UpdatedAt = new DateTime(2012,01,01) });
            var widget = new Widget(service.Object);
            widget.Init();
            var updateDate = widget.UpdatedAt;
            widget.Update();
            service.Verify(s => s.GetNumber());
            var updateDateAfterUpdate = widget.UpdatedAt;
            Assert.AreNotEqual(updateDate, updateDateAfterUpdate);
        }

        [Test]
        public void UpdateTimeShouldHandleExceptionFromCache()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache()).Throws(new KeyNotFoundException());
            var widget = new Widget(service.Object);
            widget.Init();
            service.Verify(s => s.GetNumberFromCache());
            widget.Update();
            service.Verify(s => s.GetNumberFromCache());
        }

        [Test]
        public void ShouldReturnItJsonSerialized()
        {
            var service = new Mock<IWidgetService>();
            service.Setup(s => s.GetNumberFromCache()).Throws(new KeyNotFoundException());
            var widget = new Widget(service.Object);
            widget.Init();

            var result = widget.GetSerializedAsString();
            Assert.IsNotNullOrEmpty(result);
        }

        [Test]
        public void ShouldGenerateWidgetFromString()
        {
            var service = new Mock<IWidgetService>();
            var serialized = "{\"Number\": \"0\" }";
            var widget = Widget.FromString(service.Object, serialized);
            Assert.IsNotNull(widget);
        }
    }
}
