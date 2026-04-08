using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Emily.Clock.Device;
using Iot.Device.MulticastDns;
using Iot.Device.MulticastDns.Entities;
using Iot.Device.MulticastDns.Enum;
using Iot.Device.MulticastDns.EventArgs;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.Networking;

public class MulticastDnsManager : IMulticastDnsManager
{
    private const string HostNamePrefix = "emily-clock-";
    private const string ServiceInstance = "emily-clock._http._tcp.local";
    private const string ServiceType = "_http._tcp.local";
    private const ushort ServicePort = 80;
    private const int ServiceTtl = 4500;

    private readonly ILogger _logger;
    private IPAddress? _address;
    private MulticastDnsService? _service;

    public MulticastDnsManager(IDeviceManager deviceManager, ILogger logger)
    {
        HostName = $"{HostNamePrefix}{deviceManager.SerialNumber}.local";
     
        _logger = logger;
    }

    public string HostName { get; }
    public bool IsRunning { get; private set; }

    public void Start(string ipAddress)
    {
        if (IsRunning)
        {
            return;
        }

        IsRunning = true;

        _address = IPAddress.Parse(ipAddress);

        _service = new MulticastDnsService();
        _service.MessageReceived += OnMessageReceived;
        _service.Start();

        _logger.LogInformation($"mDNS started: {HostName} -> {ipAddress}");
    }

    public void Stop()
    {
        if (_service is not null)
        {
            _service.Stop();
            _service.MessageReceived -= OnMessageReceived;
            _service.Dispose();
            _service = null;
        }

        IsRunning = false;
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e.Message is null)
        {
            return;
        }

        var answered = false;
        var response = new Response();

        foreach (var question in e.Message.GetQuestions())
        {
            switch (question.QueryType)
            {
                case DnsResourceType.A when question.Domain == HostName:
                {
                    answered = true;
                    response.AddAnswer(new ARecord(HostName, _address, ServiceTtl));
                    break;
                }
                case DnsResourceType.PTR when question.Domain == ServiceType:
                {
                    answered = true;
                    response.AddAnswer(new PtrRecord(ServiceType, ServiceInstance, ServiceTtl));
                    response.AddAnswer(new SrvRecord(ServiceInstance, 0, 0, ServicePort, HostName, ServiceTtl));
                    response.AddAnswer(new TxtRecord(ServiceInstance, "path=/", ServiceTtl));
                    response.AddAnswer(new ARecord(HostName, _address, ServiceTtl));
                    break;
                }
                case DnsResourceType.SRV when question.Domain == ServiceInstance:
                {
                    answered = true;
                    response.AddAnswer(new SrvRecord(ServiceInstance, 0, 0, ServicePort, HostName, ServiceTtl));
                    response.AddAnswer(new ARecord(HostName, _address, ServiceTtl));
                    break;
                }
                case DnsResourceType.TXT when question.Domain == ServiceInstance:
                {
                    answered = true;
                    response.AddAnswer(new TxtRecord(ServiceInstance, "path=/", ServiceTtl));
                    break;
                }
            }
        }

        e.Response = answered ? response : null;
    }
}
