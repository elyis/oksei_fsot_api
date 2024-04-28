using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;

namespace oksei_fsot_api.src.App.Service
{
    public class CreateMarkService : ICreateMarkService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEvaluatedAppraiserRepository _evaluatedAppraiserRepository;
        private readonly IMarkRepository _markRepository;
        private readonly ICriterionRepository _criterionRepository;
        private readonly IMarkLogRepository _markLogRepository;

        public CreateMarkService(
            IUserRepository userRepository,
            IEvaluatedAppraiserRepository evaluatedAppraiserRepository,
            IMarkRepository markRepository,
            ICriterionRepository criterionRepository,
            IMarkLogRepository markLogRepository)
        {
            _userRepository = userRepository;
            _criterionRepository = criterionRepository;
            _markRepository = markRepository;
            _markLogRepository = markLogRepository;
            _evaluatedAppraiserRepository = evaluatedAppraiserRepository;
        }

        public async Task<IActionResult> Invoke(CreateMarkBody createMarkBody, Guid criterionId, Guid appraiserId)
        {
            var criterion = await _criterionRepository.GetAsync(criterionId);
            if (criterion == null)
                return new NotFoundResult();

            var evaluatedUser = await _userRepository.GetAsync(createMarkBody.EvaluatedId);
            if (evaluatedUser == null)
                return new BadRequestResult();

            var appraiser = await _userRepository.GetAsync(appraiserId);
            var evaluatedAppraiser = await _evaluatedAppraiserRepository.AddOrGetAsync(appraiser, evaluatedUser);

            var mark = await _markRepository.GetAsync(criterionId, evaluatedUser.Id, createMarkBody.Date.Month, createMarkBody.Date.Year);
            if (mark != null)
                return new ConflictResult();

            var result = await _markRepository.AddAsync(criterion, evaluatedAppraiser, createMarkBody);
            if (result == null)
                return new BadRequestResult();

            var log = await _markLogRepository.AddAsync(result);
            return new OkResult();
        }
    }
}