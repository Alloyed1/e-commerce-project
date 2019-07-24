let hubUrlNotify = '/chat';
		const hubConnectionNotify = new signalR.HubConnectionBuilder()
			.withUrl(hubUrlNotify)
            .configureLogging(signalR.LogLevel.Information)
			.build();

		hubConnectionNotify.on("NotifyAdmin", function (data) {
			$("body").overhang({
				type: "info",
				message: data,
				duration: 3,
				upper: true,
			});
		});

	hubConnectionNotify.start();