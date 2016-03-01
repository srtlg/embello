\ hardware SPI driver

$40013000 constant SPI1  
     SPI1 $0 + constant SPI1-CR1
     SPI1 $4 + constant SPI1-CR2
     SPI1 $8 + constant SPI1-SR
     SPI1 $C + constant SPI1-DR

: spi. ( -- )  \ display SPI hardware registers
  cr ." CR1 " SPI1-CR1 @ h.4
    ."  CR2 " SPI1-CR2 @ h.4
     ."  SR " SPI1-SR @ h.4 ;

: +spi ( -- ) ssel @ io-0! ;  \ select SPI
: -spi ( -- ) ssel @ io-1! ;  \ deselect SPI

: >spi> ( c -- c )  \ hardware SPI, 8 bits
  SPI1-DR !  begin SPI1-SR @ 1 and until  SPI1-DR @ ;

\ single byte transfers
: spi> ( -- c ) 0 >spi> ;  \ read byte from SPI
: >spi ( c -- ) >spi> drop ;  \ write byte to SPI

: spi-init ( -- )  \ set up hardware SPI
  12 bit RCC-APB2ENR bis!  \ set SPI1EN
  %0000000001010100 SPI1-CR1 !  \ clk/8, i.e. 9 MHz, master
  2 bit SPI1-CR2 bis!  \ SS output enable
  OMODE-PP ssel @ io-mode! -spi
  OMODE-AF-PP PA5 io-mode!
  IMODE-FLOAT PA6 io-mode!
  OMODE-AF-PP PA7 io-mode! ;