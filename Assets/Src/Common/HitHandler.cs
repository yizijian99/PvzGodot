public interface HitHandler
{
    HitRequest buildHitRequest();

    void DoHitRequest(HitRequest request, HitResponse response);

    void DoHitResponse(HitResponse response);
}
