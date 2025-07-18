﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Primitives;

public record DomainEvent(Guid Id) : IDomainEvent, INotification;
