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

        public async Task<IActionResult> Invoke(CreateMarkBody createMarkBody, Guid appraiserId)
        {
            var criterionEvaluation = await _criterionRepository.GetCriterionEvaluationOptionAsync(createMarkBody.EvaluationId);
            if (criterionEvaluation == null)
                return new NotFoundResult();

            var evaluationPerson = await _userRepository.GetAsync(createMarkBody.EvaluationLogin);
            if (evaluationPerson == null)
                return new BadRequestResult();

            var appraiser = await _userRepository.GetAsync(appraiserId);
            var evaluatedAppraiser = await _evaluatedAppraiserRepository.AddOrGetAsync(appraiser, evaluationPerson);
            var date = DateTime.UtcNow;

            var mark = await _markRepository.GetAsync(createMarkBody.EvaluationId, evaluationPerson.Id, date.Month, date.Year);
            if (mark != null)
                await _markRepository.RemoveAsync(mark.Id);

            var result = await _markRepository.AddAsync(criterionEvaluation, evaluatedAppraiser, createMarkBody);
            if (result == null)
                return new BadRequestResult();

            var log = await _markLogRepository.AddAsync(result);
            return new OkObjectResult(result.ToMarkBody());
        }
    }
}