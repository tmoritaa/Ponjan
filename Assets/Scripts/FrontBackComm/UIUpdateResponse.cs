﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIUpdateRequest {
    public enum UpdateType {
        None,
    }

    public UpdateType requestType = UpdateType.None;

    public UIUpdateRequest(UpdateType requestType) {
        this.requestType = requestType;
    }
}
