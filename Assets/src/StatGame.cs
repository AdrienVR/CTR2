using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class StatGame
{
	// class to store the statistics of a game
	
	public Stopwatch chronometre;
	public Dictionary <string, int> score =  new Dictionary <string, int> ();
	public static List <StatPerso> listPerso=  new List <StatPerso>();

	// portable string css for webpage export
	public string css="<style type=\"text/css\"><!-- body {font-family: Verdana, Helvetica, Arial, sans-serif;font-size: 14px;color:#C0C0C0;background-color: #000000;margin-left:10%;margin-right:10%;Line-Height: 2;}h1,h2{background-color: #0C0C0D;color:#FFFFFF;text-align: center;}img{vertical-align:middle;}--></style>";

	// Base 64 encoded images for webpage export
	public string img1="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcEAQ3siRV1gAAAUpJREFUOMutVLFuwyAQPcAiC13IYGWLMmX1aP+B//8vrC6tVSUh4V0HY5caSFMlT2IB7nF373HCe88UIISgZ1ERESml6BUAMBHmwMx3g0vVVH8R7fd7N47jl1IKRPQ2DIOO7yXEAJiIeLrzA631bd7v+37kiWFeiO/O+wC4ymUWXl0aq7XmtGKxxDDzkqlcl7zZbPyjIiilkkYnhM459ahAAMRdwsPh4HKB3peT3m63t6woobmfUeMTsYgImfPzLAwAXpec1GatfY/FyFm2mGFd15f49d1u97G2xnoZY65xhgIASykTo0op2Xsvwp6PbZQTTAhBAFKVQ5+ImUWwBkpkOSSEXdedY7MCkKVga+0t56VEzePxeJJSotQ3IoK19pr7er8I1/+5bdtLyOJMRCdjzLVpGpf7x4ko/xldpRFWnIfPTO5qZn4VvgGUvDNUjlPwkQAAAABJRU5ErkJggg==\">";
	public string img2="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QABACeAJwC91cqAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wQLFB0k1X2m+gAAA0ZJREFUOMutlE1oXFUUx3/nvjeZzCQz5mPS2kmTTGpsbdrqwhgpLqQ0pSK4MCgouvBjW5FE3LhypYuCFhTcSXAjLgR3QdSFiihlilooDYXYYHTAJG0mma/MvPvucfHeTKa68uPA41549/7vOb//uVfCMFTiEBH+a/gA3tvmH216auoTnp76mAdyPzLWv46IErgEmeEm5t9kUcjc5NdKgbXKEWq2D4CECfYzbMeJHOTSkPRgJg+bNfh5I/p3uQQoILB0/WWemPyM8xPL/LJ7D9OD1/CNjQS7uT17As4U4OgQBCG8V4SBJJSbbcjRsLU3QtJr8tH1F/mjcZCLjyyQ8uv0AuKcU/NWtPLCDBwfhtt70Ajg9GEwAt+tw5dr+1Vc24oyzvVusnTuOUq1UebGPqcw+vudJb9f/Duv+4ZhPBvNB3shn4FSNWaZvcmln17j/PgyK9vTFEZjhuKXOzVpuzYVBFgpw0pZmD1kmMn7jKQFTAjGceXWKVBhLLvKeu0Qj51sm5LYiYAjHdtVBQREY3BegmRPivljfTw6AcdGmvxQCpjMeizdmAfg0uPtDL1tFAER8nMPl/yU5lxLmuvLlzORaUJxC04fHuLKBkwN9PDSqSSzefj6tzojmSqbjRA4GJnif/ANosLkk3Panw86tm9eTa0d+PadwgvepyyGb8ZdI6jCQwcyABQ3amhcnb0wu89w+P7nv+jPB+e6DRk8uldIfr/N3ZTAL8flR2yL2zugAn7UTRqjiRj6ZdIjwZluMXVgDHzovaFW0yJmJ8KssaRInFnMXrvusoglO97ynRXUCi4UNITQGpwXSGANmEbc2drVDcpfnxMDMHT81at2z9CqGFpVj1bV0Kx4BBXDKw/2MD/tym0ZiYVAGbx3od6XX7yoXdfIAPQOhyeDiqFV8WlVDcGuR6sSfTfUJ0gx0EERjwNHFmpBzUurNa9nxxdDF0RLxDmnk6tp1NFVruACwTlBLWgogO6WV9/NAmQKC18FO/5ZFxrUCmEIIkrjmc2Ioa15OEsk1ha1ggvB2egAVbIKYLM0byfO2roXHawRijYMH6BVMZ2NbUOcFZw18RxcaLhrYrHerHqp5q2EcOcDtD8655T/Mf4EgmtoLRQcLvIAAAAASUVORK5CYII=\">";
	public string img3="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFDEiesHbVwAAAuBJREFUOMutlE9vG0UYxn8zs7Z3ba/ttUmamrSVhSohGSoqpEqBUyQ4ceTIhc/Fjc/AhWPEgVKpRVWiiqQQKqEEp3bd+u96d707LwfXiWnjCATPaTQz+ul5/6osy4TXUkrxX+UA7O7uPnnzYQkXkX8M29vbazsAWuv28tLzPObzOTdv3sJxDMfHx2RZ9u8cLhXUAj68c4cXvR71oE4jqPHns9+ZpAJKnbu9KjXOamhplqGVon19C33wMx/MQ66fHPKgGPDQq5JZizEGrfVaqLOas/F4xP79H/ly3OOj4wNaOmNHCbfHLzkyAb/mi5TLZXzfXwvUS3ciAiK82/mDjaMDTJpiraCB9jzki9FzzPAVg8GAJEnWFksDJElCHEVs9p/zSe+EqqSkwPw8L/C5SbkXT4jDkOlwSBRFf4Muzw7A2dkZrusST0YEsymOBgskFqwGrWDbwFeS0Iun3H05o58TjlwXESGOY5IkuQCGYYi1lt9E8Z0bcCuLuZ1G5CXDUeAbKGi4Zyxfp1N2UuF0rPnGLfKLGE47HdI0XdTCWiutVovJZIJSitb2NpVCgY3xgJ1+h514RNNAxYABns2hrOGaA48KFR6Q4+lwyv1M83g0WTis1+s4joPruri+T2YMHdfle69Et3vCp6MeN6ygFcQCBYFU4ONoxPsC+wr2KV6EXKlUzlthtR2mpTI/bN7gVDSfjV/wHik5tchvLJDTUAAOPZ9+oX4BfBMkIiRJwnA4REQY+AENO+furI+nFkVKBLopPLaGnxpbXHtn6+3RW1UYhnS7XYrFIqVSiUO3xKNoTH4+JwEGGE7zHk8bG7xqbGKMuRroOA5KKWazGcYYDjF8G2zj5fNYpUhzOVLXIysUUFpfvhxWx9DzPHzfZzgcEoYhIkK/2aRarS7+AHLJoljr0BhDrVYjiiKUUlSr1bdmWK1bDtbaJ5dBy+UyzWYTx3HI5/O8/nvVOmwra63wP+ovNH1DO9nr590AAAAASUVORK5CYII=\">";
	public string img4="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFDQXUQXrMQAAA2VJREFUOMutlEtvHEUUhb9bVT090z0znpfHJgkRZhHYwCYRUljDX2HNr+FvBIkNEptIRIggJKQskBKQjIEkxLHHnlc/pqvqshgHD2s40t1+OnXuqSshBOVKIsJ/lQN48Pn9RT+ldRFSFEisYlHQiEbAgKBoVJqgSFScVZxErBguG8fCGz774lHbAYwy6Y86gm4sVVAMEYtiEIiKsJ0AOFUwSiKQOENiBRxYL9cORQSNUK9rLpY1xXrFqFOy1/dkeSBNFImwKYWiNKxXlpVvkY76JGmLOkSwZgvc5mYIGqirhhZw43DFe5++oncUsZkiFlDQDfglFH9aLp52mL10vF4plYd82L12CIJXWBUlqQhv3ymZfBSQFFSvAxeBZArtdwPDuyuG3/3B5YMhazei7XaWEoGAITSeoBXZQYEqxAuhuTRslooALldaI8UMwHRh727F+FHJemFIrV4DN0GxBryCbTzLYyjPu2g5ZLls89dJgV/WHB5Ybt9bMPikRPIr2xoRVYz6HaD3pM5RBGVWWh5+O8C4lHeOBkxHwnivQvINBx9E8vdrJANtYP6T5fyFQzNPDGHnyVFRIAAbMcyalKSJZJvfuf1hQTL12GHEDEEcxDmcP7Y8+zpjViR0OoroToYYQVXJDNuNIrRVmRwV5Pc34EArCKdCdWx59WOLk58zZusW1hqSq9npoQVVcgFnQIFMIvkoggWtYfZNm9Nf9qmXGa9PPavaI04QEaxzOOeugdYIqhEfFR8iAJ3MYCtD8cRSz5STxynP11264y7JNODsmvW8gqgEBTFmtzaCR1gGpQzQzxOyYYf5Mzj/tWBRbDh9aTir51RNZHzQZ3zYxxplfrYCDdhdoCoEFUDotB2TUZfRqGJ4b05+p0Bt5OyHDd9/abi4MBiBGzf2mLw1IHEWFSHZ7WGDYDBM8oRunjHpJUxvPWfw8QrpbJvf3Y/0WoIPQpyXLJxheNhnenOI33gSYbfYllaiHEz7dNuGZlkwfxHoPTW4gbL6zXH8MGddtnFGQCyLeU2QFZPDLr1OgvsXEEGNxWYGVU8jjrPzmzRf7WOSSFU6qjphfMv8c4iNbJeZi9AycvWB32QopoqqaREswQpuz5GaHkEiQRXbVvZEEARQrBESURKzhfqgb66ISIxR+R/1N8kgnG3dqh/nAAAAAElFTkSuQmCC\">";
	public string img5="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFDYlq+TYMwAAAgBJREFUOMutk01u1EAQhb+qtuePZEJEJAKX4FosOBQn4DDs2CCEAGkipIghMJOxux8Ld497nLCCtizb1VWv3ntdthijyMvM+NfVALx784rbQ8tdDBiJmQsl4QgD+iTaAH0UMQoziDExDxBcuEGnwOu37wdAybjb7vhw07HrEtfnDZvtgSetsZ4Hvm07XqwbbncRE1wsnC+3B67XLfdRdBIvny1HhpLY7zo233+x3UeWWvB5s+Ny7sSzhk839yy04OuPjgbQuuHjZk8j8fOQ+N2L9WoGgJsZCSMVHwHP74JjvOwKIQ15QwaYBnsotePnkFjukhxUinVsZareqwN1gChIGootF9oUXKWhjSok0MDbaoZRZBkDkApQJuXFCgnTEPcMXGo4lTxl85ChVaxra3yiwJkcRin2enNyWKN3GgEzYlOsAAhAQPk5simA4RF2nmu8/lOSBhNDDjjg4oFsf0RNaR6skiyBWS15+D7eNUhlZA3sNvFQtThjIjaHzU5j+WmUThkw5f/ZJ7IKN6/mz/LlGGaG26iumsNxKOw4viPzv8fIDa0QHAB7GTI7+pOq9JT9qcSRjtyt8ryS3OchKZuq7FNFyet944S31WMjc2bLGVdXK877xOW6RebMW2MxD7SNc/W0ZbVo6QWrpdME4/lFS+rFIYmLs3YATimJ/7j+AO4jzr6s91r7AAAAAElFTkSuQmCC\">";
	public string img6="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFDsJLJLKnQAAAJJJREFUOMtj/Pv3738GKGBkZGSgFLAwMDAwTJ48mYEaIC8vj4GJgcqABZ9kQUEBVvEJEybgNhBfuB0SDMEqfo5cF3K8/0FdL5MaFHl5eeQZ+GqmNHVdKMr8DKv4f5iBuLxwisGHzDC0XYhd9vBqkg2kesIeNXAkGIg3p/wQ5MAq/us/G1ZxVgYGBsZ///79p6YLAfkHIBOVPqtRAAAAAElFTkSuQmCC\">";
	public string img7="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFQgt5TghSwAAAyxJREFUOMutlEtoXGUUx3/fvZOZySQzmWRilWQsKr6CVeqrC8VWLGqMtpviwkIVRReCFlFQFz6KKIIgiKu6saBC62tjwaXUB01LwQTTZGaSaGgSSWOacZLm3iT33u8cF+kkmTzqQv9wNt/5vv855+N//sZaq1yCMYb/ihiA67ob5SxgVsW/QkSWCDeEixPfnsI0u0amInRBkAsRUrFLpS7X4UaoP5gzj7y4l1tzHfjRPKGJOM2vDB4fYObDCaLfFjZtU4GacDpT+sLw6zoUjWgggYYSaiShBhLqQDSoXaP71XQm170TEV1PGENzZ27UYjSkKqpWbE2oqvZqv+76pFPJryd01nZsWlxaYzl8ncdu8lkLLKBZILXuOdWTahVMaBgvjdLnFZjVOebURxAiLBfVY1FDTkc9jM9MYIJlwhXpiYg6jiOryEk+2ETT2204GZe55DxH2w/zh57j8NlPmW6ZwZQF72gZ71gZmQyrhKZWNgbidzcgcxbxLYs/zeId+5vwZuGv9yfpuaafsXdKBBKQ3JEm6PVxW1yMC3YiVHRJq9UOca+q4+ofbiPf2s7w8DA7tt5JfaKeJpOhEA7S4jYTo462+BZOFk5R6Cmw/d7bqRSnKb7VI1IKHBFZGVMb4Jb4TbzS+Dz3d+yk+8duzlXG2J25D+9khfNTEzya3k1xpMRAucRjD3Wxv+Nx8q3tSFbN2j9UgOQDGZN6Lse1O6/nzzNjTB+f4MBLT9J1w8O0aJZ3/Y/o/uAEjfc0YxIG76syiydmsWOhaqhGRFZGXq6QMLjXJYlvq8fdEiP1VI6GfJrQDwimAha+ryCzlsVfLhL0+CC1u1wltIBj0q5pOJCj4Ykcia1JPPV5LXuQttiVoFCyv/PN+HfkM+2U4xUGPzuL//EUdjxcJnSWrQAw2+r0mVef5b273iA1Emfu0CRfDH1JnxTpo8jXP3/L0JF+YqPw5hUvs2ffHuwdsQ11uGwV2UN5xLN4Ry5gz4eYjIubi4GCnQzQeSWxK03j060EvT7e59PIdLRu5BUPNJhLmlrrgdVtWso5q/drcz90V19Yu+Y1ReQyfigi/F/4BzjCwg/IOQ3BAAAAAElFTkSuQmCC\">";
	public string img8="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFQokrtL7bQAAAylJREFUOMutk+1PW1Ucxz+nt729vaV3K1spo4Azw4jDinNkGuYSEwKbGOML3znjG/8K/wb/Cd+aLCabyTTRLGTTaJxBNpWQgIzSBygFVu4dbe/DuccXUlewuBf6Sc6r8zvf8/09CSml4gAhBP+VKIBlWd3uJCA6zjNxHOcvwW4YnowMWxYJLSoc3yUIFXbg0/J8Wrr27w67MZUbEh9MXKJvcAjf9xF+QGt1jW9qFb4qrPLIb3UX7Fa3V40k16enyV+dQWQHkDENMNH8bUaKVd78/Aaf3rvDL+7+sx0anuT9sVHyU1M0RkdQqt2zADiJSKe54L3LpN+gMjdHVchD7yNHBZOxKLrVQ+j5GE/crmlJGWJGNGJdsms7bNsQUkB5Y5Odcpn+TJqoLQisU2h+A9FsIowY9uoa9tYWUqj2S9WehE7BCEAdxd3tTdI3b2F+eRvNd5n86Dp+dYfvfvyBRNPFkQHFWg1fhcfXUPcCzloncGWAF0qW6rvcr1bIajGe398n82idL5Z+Q/k+5070Uty3MfU4fUFAvdVSnh4VACIMQ5VKpTgd0fjk4mUGjQRL9V1GB3IYepyIofNkc4t40kRoAuNkL/OlAisry4y/OMq6bXNj/qdwPQwijuM8bUpcCTLpNMPXpnjt7HPcXV1m094jO3ae+aBFqdUgm3+F5VqVx4UiUxcmeH18nHQmgxnR/u5O26ECyCct8Vb/IBP9OR7u1lioFPlw5m1eyr+MSJpUbn3NZ4sPGOk9RUzTuF/d4Pe9XR67rnI1IRzHeZpym4RU9BlxBnssUrE4l7MDZBImrTBkz23wcHeHpu+zYtdZs/fw9GjXXZZAJCVDMdmf443+HKcNk9DzGLsySTydRglBc6NKz+IiZ+I6yg24vbbCt5slqtL/x2BrALmEqT6+OsvM7Cw7QnGz8Ac/LzygVdrAWy9zZ2GeucVfKRlxzl+b5p2LlxiO6p1zfDhlw5O8d+4FvCDgXrXMdiixwhBTjwOw53o0NUE+meLKmSEKjs331TL1A71uNZS6F4iDmTq6V6pjI4TuBQDH1rCN1hlwhEOfHBcnwjBU/I/8CYD9dC+lc0BNAAAAAElFTkSuQmCC\">";
	public string img9="<img src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAB3RJTUUH3wMcFQkuZSpBsAAAAytJREFUOMutlM1vG0UAxX+ztndtr71O0sRGkAaiJIQS2iIQAjVVoQWpSI1EkOiFayVQD0gIiT+BA38AnGhP5YC4VjkgwgEJWopAoiRqQhOSRk5a3ObL6/V+zgwH6tRJHXqAJ81lZvTmvTdvRkgpNfchhOC/Ig3gOE6nNQmItvFIuK77D2EnFL3IGLEsChhiUyfEWrOpNc1E4trmvyvshLM5R5wfGaV3YJAkijCiGGNmjq9kk0uNe8yYnUULpZQuFou7Jk/6kk9eP83Q25P4gyOEuQzQg+UvkJ9bYe3zC3y0cJ3v8qlHWy56Ee93P8HwxARLJ15C69adBUA/4uBBnvUajF/Os3T1Kkv2blJjL6FjgNFTRIchXfe8jrak1BQ1mOh9M2ytiMQQrN6+y/atFZz+MoUaNMpPYvkbZFwXbVvI2Xm822skxg6hbjWhlaFqV/uqNDhXLFPAwPJdxj44j7Fyh++v/UT3Zp0NA75tbjPtb1HNmjuEuzK0vZBRK0ugFKGh+DVqMu1v8VSk+TiKOPrbDS7euQVRzJiVZzH2KaRNBiLJepxoz7Z2KaQ/knxWGeJpZfBL3ORIb4WsaSJyWYLqGmbBhjRkDpT5Yb3GzcV5Xhge4WYYcXFhVt3IZQzXdR/YzElNpdRF4d1Jjj1WYaq2SrXZoPTi83zTlWUBRfHlV5ipb9FY+JMzh44yPvYcXeU+HPXgJbUUaoBxmRKT2RKvFbq5Ejb4cavGe8dP8cyRw8iSQ3zhSz5drzJmO1hCMO3X+Tn2qSmlPSslXNd9uNhOkNBvwFAmSyllcsZyeDxt4gnYSHyuhAENJfk99pkPAzzb6lhsCRgH/EC8mS9xOtdNJWMh/IChUydI95ZRhkAtr2DPzTGoFcJPuFS/y9dhnWXTeKjYKYBh0vrDYyd5Y2KC1VyGL5obXLs+g15cIjW/yNTcLFPLfzBb6WPgnbc4e+gwo7Fs7/Fuy0Uv4lypl0ApLgfbVM0UfX5AIW0CmnWpqWfTHE8EE/ke5mOfqWCbv6zMjuW9GUrbC8X9Tu39TnTbixC2FwLsm2ELqfYNe3+m9kP22yeUUpr/EX8DI7h3/jyYwwgAAAAASUVORK5CYII=\">";

	// DATA :
	public string map;
	public string duration;
	public int nbPlayers, nbAppleBox, nbWeaponBox;

	string tr(string original)
	{
		return TranslationResources.GetTraductionOf(original);
	}

	public StatGame(int n)
	{
		nbPlayers = n;
		initStatPerso (n);
		map = Application.loadedLevelName;
		// args : string m, int nbPlayers
	}

	public void startGame()
	{
		chronometre = new Stopwatch();
		chronometre.Start();
	}

	public void pauseGame(bool activ)
	{
		if (activ)
			chronometre.Stop ();
		else
			chronometre.Start();
	}

	public void endGame()
	{
		chronometre.Stop();
		TimeSpan t = new TimeSpan ();
		t = chronometre.Elapsed;
		if(t.Hours == 0)
			duration = String.Format("{0:00}min, {1:00}s", t.Minutes, t.Seconds);
		else
			duration = String.Format("{0:00}H, {1:00}min, {2:00}s", t.Hours, t.Minutes, t.Seconds);

	}

	public void initStatPerso(int nbPlayers)
	{
		for(int i=0;i<nbPlayers;i++)
		{
			listPerso.Add(new StatPerso(i+1));
		}
	}

	public StatPerso getStatPerso(int numeroPlayer)
	{
		foreach(StatPerso p in listPerso)
		{
			if(p.numeroPlayer==numeroPlayer)
				return p;
		}
		UnityEngine.Debug.LogError ("erreur: il n'y a pas de StatPerso numero " + numeroPlayer);
		return new StatPerso(-1);
	}

	public string[] getReport(bool export)
	{
		List <string> lines=  new List <string>();

		if(export) lines.Add("<html><head>"+css+"<meta charset=\"UTF-8\"><title>CRASH TEAM RACING II</title></head><body><center><h1>");
		lines.Add("CRASH TEAM RACING II");
		if(export) lines.Add("</h1>");
		lines.Add(tr("Partie jouée le ")+DateTime.Now.ToString("dd MMMM yyyy à H:mm"));
		if(export) lines.Add("</center></br>");
		lines.Add("");
		if(export) lines.Add("</br>"+img1);
		lines.Add(tr("Durée : ")+duration);
		if(export) lines.Add("</br>"+img2);
		lines.Add(tr("Map : ")+map);
		if(export) lines.Add("</br>"+img3);
		lines.Add(tr("Nombre de Joueurs : ")+nbPlayers);
		if(export) lines.Add("</br>"+img4);
		lines.Add(tr("Caisses d'Armes : ")+nbWeaponBox+tr(" caisses obtenues"));
		if(export) lines.Add("</br>"+img5);
		lines.Add(tr("Caisses de Pommes : ")+nbAppleBox+tr(" caisses obtenues"));
		if(export) lines.Add("</br>"+img6);
		lines.Add(tr("Scores :"));
		if(export) lines.Add("<ul>");
		foreach (string key in score.Keys)
		{
			if(export) lines.Add("<li>");
			lines.Add(key+" : "+score[key]+" Pts");
		}
		if(export) lines.Add("</ul>");
		foreach (StatPerso p in listPerso)
		{
			lines.Add("");
			if(export) lines.Add("</br><h2>");
			lines.Add(tr("JOUEUR")+" "+p.numeroPlayer);
			if(export) lines.Add("</h2></br>"+img6);
			lines.Add("\t "+tr("Score : ")+p.score+" Pts");
			if(export) lines.Add("</br>"+img7);
			lines.Add("\t "+tr("Points Marqués : ")+p.PtsMarques.Count+" Pts");
			List<int> traites = new List<int>();
			if(export && p.PtsMarques.Count>0) lines.Add("<ul>");
			else lines.Add("</br>");
			foreach(int j in p.PtsMarques)
			{
				if(j != p.numeroPlayer && !traites.Contains(j))
				{
					if(export) lines.Add("<li>");
					lines.Add("\t\t "+tr("Contre Joueur ")+j+" : "+compte(p.PtsMarques,j)+" Pts");
					if(export) lines.Add("</br>");
					traites.Add(j);
				}
			}
			if(export && p.PtsMarques.Count>0) lines.Add("</ul>");
			if(export) lines.Add(img8);
			lines.Add("\t "+tr("Points Perdus (suicide) : ")+p.nbSuicides+tr(" Pts"));
			if(export) lines.Add("</br>");
			if(export) lines.Add(img9);
			if(p.PtsDonnes.Count==0)
			{
				lines.Add("\t "+tr("Points Donnés (mort) : ")+p.PtsDonnes.Count+tr(" Pts"));
				if(export) lines.Add("</br>");
			}
			else
			{
				lines.Add("\t "+tr("Points Donnés (mort) : ")+p.PtsDonnes.Count+tr(" Pts")+tr(" (pire ennemi : Joueur ")+most(p.PtsDonnes)+")");
				if(export) lines.Add("</br>");
			}
			if (p.getTotalWeapons()>0)
			{
				if(export) lines.Add(img4);
				lines.Add("\t "+tr("Détail des Armes obtenues : (")+p.getTotalWeapons()+tr(" au total)"));
				p.printWeapons(lines,export);
			}
			if(export) lines.Add("</br>");
		}
		if(export) lines.Add("</body></html>");
		string[] linesArray = lines.ToArray();
		if(export)
		{
			string date = DateTime.Now.ToString("ddMMyyHmm");
			string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			System.IO.File.WriteAllLines(Path.Combine(desktop,"CTR2_"+date+".htm"), linesArray);
			UnityEngine.Debug.Log("A report of the game have been exported in your desktop !");
		}
		return linesArray;
	}

	public int compte(List <int> liste, int element)
	{
		int occurence=0;
		foreach(int e in liste)
		{
			if(e==element)
				occurence++;
		}
		return occurence;
	}

	public int most(List <int> liste)
	{
		int deathByJ1 = compte (liste,1);
		int deathByJ2 = compte (liste,2);
		int deathByJ3 = compte (liste,3);
		int deathByJ4 = compte (liste,4);
		int maxDeath = System.Math.Max (deathByJ1, System.Math.Max(deathByJ2, System.Math.Max(deathByJ3, deathByJ4)));
		if (maxDeath == deathByJ1)
			return 1;
		else if (maxDeath == deathByJ2)
			return 2;
		else if (maxDeath == deathByJ3)
			return 3;
		else
			return 4;
	}

}



public class StatPerso
{
	// class to store the statistics of a character

	public int numeroPlayer, score, nbSuicides;

	public int nbBomb, nbMissile, nbTNT, nbNitro, nbFlacon, nbAku, nbAccelerator, nbShield;

	// Cette liste représente les points marqués par le joueur.
	// les nombres qu'elle contient sont les numéro des énemis tués.
	// ex [2,2,2,3] signifie : 3 points contre J2, 1 point contre J3
	public List <int> PtsMarques=  new List <int>();

	// Cette liste représente les points donnés par le joueur (points marqués par les énemis contre lui).
	// les nombres qu'elle contient sont les numéro des énemis qui l'ont tué.
	// ex [2,3,3] signifie : J2 l'a tué 1 fois, J3 deux fois.
	public List <int> PtsDonnes=  new List <int>();

	public StatPerso(int n)
	{
		numeroPlayer = n;
	}
	
	string tr(string original)
	{
		return TranslationResources.GetTraductionOf(original);
	}

	public void printWeapons(List <string> l, bool export)
	{
		if(export) l.Add("<ul>");
		if(nbBomb>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Bombes : ")+ nbBomb);
		}
		if(nbMissile>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Missiles : ")+ nbMissile);
		}
		if(nbTNT>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("TNT : ")+ nbTNT);
		}
		if(nbNitro>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Nitros : ")+ nbNitro);
		}
		if(nbFlacon>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Flacons : ")+ nbFlacon);
		}
		if(nbAku>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Aku-Aku : ")+ nbAku);
		}
		if(nbAccelerator>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Turbos : ")+ nbAccelerator);
		}
		if(nbShield>0)
		{
			if(export) l.Add("<li>");
			l.Add("\t\t"+tr("Boucliers : ")+ nbShield);
		}
		if(export) l.Add("</ul>");
	}

	public int getTotalWeapons()
	{
		return nbShield+nbAccelerator+nbAku+nbFlacon+nbNitro+nbTNT+nbMissile+nbBomb;
	}

	public void addWeapon(string w)
	{
		switch (w)
		{
		case "greenShield":
			nbShield++;
			break;
		case "blueShield":
			nbShield++;
			break;
		case "greenBeaker":
			nbFlacon++;
			break;
		case "redBeaker":
			nbFlacon++;
			break;
		case "bomb":
			nbBomb++;
			break;
		case "triple_bomb":
			nbBomb+=3;
			break;
		case "missile":
			nbMissile++;
			break;
		case "triple_missile":
			nbMissile+=3;
			break;
		case "Aku-Aku":
			nbAku++;
			break;
		case "TNT":
			nbTNT++;
			break;
		case "nitro":
			nbNitro++;
			break;
		case "turbo":
			nbAccelerator++;
			break;
		default:
			break;
		}
	}

}



