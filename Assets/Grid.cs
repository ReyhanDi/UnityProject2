using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GameObject square, X;
    public int n , saven;
    Vector2 pos, xobjectspos;
    int row = 5, col = 5, cellsize = 1 , r ,c , counter2 = 0;
    float sqwidth, sqheight;
    private GameObject[,] grids, xobjects;
    private bool[,] hasclicked; int[,] counters ;
    bool haschanged;
  
   
    
    // Start is called before the first frame update
    void Start()
    {
        n = 5; // matrisin başlangıç satır ve sütün değeri
        saven = 0; //Unity platformunda Inspector kısmında n'nin değeri değiştirilip değiştirilmediği kontrol edilmesinde işe yarayan değişken
        sqwidth = square.GetComponent<Renderer>().bounds.size.x;  //matristeki her hücrenin genişliği
        sqheight = square.GetComponent<Renderer>().bounds.size.y; //matristeki her hücrenin yüksekliği
       
    }

    // Update is called once per frame
    void Update()
    {

        if (n != saven) 
        {
            if (haschanged)  //satır ve kolon sayısının değeri n değiştirildi mi
            {  
                //yeni n değerinde grid çizilmeden önce var olan grid yok edilsin
                for (r = 0; r < saven; r++)        
                {
                    for (c = 0; c < saven; c++)   
                    {
                        Destroy(grids[r, c]);           
                        Destroy(xobjects[r, c]);
                    }
                }
                haschanged = false;
            }
            else
            {
                //yeni n değerinde en başta veya tekrardan grid oluşturulsun
                grids = new GameObject[n, n];   
                hasclicked = new bool[n, n];
                xobjects = new GameObject[n, n];
                counters = new int[n, n];
                drawTheGrid();
                haschanged = true;
            }
          
        }
      
        clickOnSquare();
        remove();
    
    }

    public void drawTheGrid()
    {
      
            for (r = 0; r < n; r++)          //hücreler satır sayısına göre çziilecek
            {
                for (c = 0; c < n; c++)     // sütün sayısına göre çizilecek
                {
                    float posx = c * cellsize;    //hücreler kolon sayısına göre 1 birim kaydırılarak boyutlandırılıyor
                    float posy = r * -cellsize;    //hücreleri satır sayıısna göre 1 birim kaydırılarak boyutlandırılıyor
                    pos = new Vector2(posx, posy);
                    grids[r, c] = Instantiate(square, pos, Quaternion.identity);  //gridler matrisin satır ve kolon sayısına göre oluşturuluyor
                    hasclicked[r, c] = false;   //sistem ilk çalıştığında hiç bir hücreye tıklanmamış sayılsın
                }
            }
        saven = n;
      
    }

    public void clickOnSquare()
    {

        for( r = 0; r < n; r++)
        {
            for( c=0; c < n; c++)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousepos;
                    mousepos = Input.mousePosition;
                    mousepos = Camera.main.ScreenToWorldPoint(mousepos);

                    //Burada her bir hücrenin tıklanıp tıklanmadığı kontrol ediliyor.Box collider kullanmak yerine kendi yolumu kullanmau-yı tercih ettim
                   if(mousepos.x > grids[r,c].transform.position.x - sqwidth/2
                        && mousepos.x <grids[r,c].transform.position.x + sqwidth / 2
                        &&mousepos.y > grids[r,c].transform.position.y - sqheight/2
                        && mousepos.y < grids[r,c].transform.position.y + sqheight/2
                        )
                    {
                      
                        hasclicked[r, c] = true;  //tıklanan hücreye tıklanmış olarak varsayıldı.
                        if (hasclicked[r, c])   //r satırlı c kolonlu hücre tıklandıysa işlem yapılsın
                        {
                            while (counters[r, c] == 0)   //bir hücreye ikinci  tıklanmada nesneyi üretmemek adına while döngüsü kullanıldı
                            {
                                xobjectspos = new Vector2(grids[r, c].transform.position.x, grids[r, c].transform.position.y);
                                xobjects[r, c] = Instantiate(X, xobjectspos, Quaternion.identity); //tıklanan hücreler ile aynı konumda X nesnesi yerleştirildi
                                counters[r,c] = 1;
                            }
                        }
                    }
                }
            }
        }
        
    }

    public void remove()
    {
        if (n == saven)
        {
            for (r = 0; r < n; r++)
            {
                for (c = 0; c < n; c++)
                {
                    //dikey olarak 3 defa yan yana gelen çarpıları yok eden işlem
                    if (r < n - 2)
                    {
                        if (hasclicked[r, c] && hasclicked[(r + 1), c] && hasclicked[(r + 2), c])
                        {
                            int s = 0;
                            while (s < 3)
                            {
                                hasclicked[r + s, c] = false;
                                Destroy(xobjects[r + s, c]);
                                counters[r + s, c] = 0;

                                s++;
                            }
                        }
                    }

                    //yatay olarak 3 defa yan yana gelen çarpıları yok eden işlem
                    if (c < n - 2)
                    {
                        if (hasclicked[r, c] && hasclicked[r, (c + 1)] && hasclicked[r, (c + 2)])
                        {
                            int s = 0;
                            while (s < 3)
                            {
                                hasclicked[r, c + s] = false;
                                Destroy(xobjects[r, c + s]);
                                counters[r, (c + s)] = 0;

                                s++;
                            }
                        }
                    }


                }
            }
        }
    }


 
}
