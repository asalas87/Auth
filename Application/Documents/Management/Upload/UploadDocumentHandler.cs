//using Domain.Documents.;
//using Domain.Documents.Interfaces;
//using Domain.Primitives;
//using ErrorOr;
//using MediatR;
//using System.Reflection.Metadata;

//namespace Application.Documents.Management.Upload
//{
//    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, ErrorOr<Guid>>
//    {
//        private readonly IDocumentRepository _repository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IDocumentStorageService _storageService;

//        public UploadDocumentHandler(
//            IDocumentRepository repository,
//            IUnitOfWork unitOfWork,
//            IDocumentStorageService storageService)
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//            _storageService = storageService;
//        }

//        public async Task<ErrorOr<Guid>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
//        {
//            var path = await _storageService.SaveFileAsync(request.File);

//            var document = Document.Create(
//                name: request.Name,
//                path: path,
//                description: request.Description,
//                uploadDate: DateTime.UtcNow,
//                expirationDate: request.ExpirationDate,
//                uploadedByUserId: request.use, // Reemplazar con el usuario autenticado
//                recipientUserId: request.RecipientUserId
//            );

//            await _repository.AddAsync(document);
//            await _unitOfWork.SaveChangesAsync();

//            return document.Id;
//        }
//    }
//}
