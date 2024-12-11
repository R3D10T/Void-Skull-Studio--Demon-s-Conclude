using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Book))]

public class AutoFlip : MonoBehaviour
{

    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Book ControledBook;
    public int AnimationFramesCount = 40;
    bool isFlipping = false;

    private bool autoFlipping = false; // New flag for entire auto-flip process

    void Start()
    {
        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }

    void PageFlipped()
    {
        isFlipping = false;
        // Only set interactable to true if not in an auto-flip operation
        if (!autoFlipping && ControledBook != null)
        {
            ControledBook.interactable = true;
        }
    }


    public void StartFlipping()
    {
        if (ControledBook != null)
        {
            ControledBook.interactable = false; // Disable interaction during flipping
        }
        autoFlipping = true; // Mark auto-flip operation as active
        StartCoroutine(FlipToEnd());
    }

    public void FlipRightPage()
    {
        if (ControledBook != null)
        {
            ControledBook.interactable = false; // Disable interactable during flipping
        }
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }

    public void FlipLeftPage()
    {
        if (ControledBook != null)
        {
            ControledBook.interactable = false; // Disable interactable during flipping
        }
        if (isFlipping) return;
        if (ControledBook.currentPage <= 0) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }

    IEnumerator FlipToEnd()
    {
        yield return new WaitForSeconds(DelayBeforeStarting);
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    yield return StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    yield return StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }

        // Auto-flip operation complete
        autoFlipping = false;
        if (ControledBook != null)
        {
            ControledBook.interactable = true; // Enable interaction after all pages flipped
        }
    }


    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        isFlipping = true;
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        isFlipping = true;
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
        PageFlipped(); // Mark page as flipped
    }
}
