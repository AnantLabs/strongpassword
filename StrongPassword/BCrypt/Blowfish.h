/* $OpenBSD: blowfish.c,v 1.18 2004/11/02 17:23:26 hshoexer Exp $ */
/*
 * Blowfish block cipher for OpenBSD
 * Copyright 1997 Niels Provos <provos@physnet.uni-hamburg.de>
 * All rights reserved.
 *
 * Implementation advice by David Mazieres <dm@lcs.mit.edu>.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. All advertising materials mentioning features or use of this software
 *    must display the following acknowledgement:
 *      This product includes software developed by Niels Provos.
 * 4. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/*
 * This code is derived from section 14.3 and the given source
 * in section V of Applied Cryptography, second edition.
 * Blowfish is an unpatented fast block cipher designed by
 * Bruce Schneier.
 */
#pragma once
#include "Type.h"

#define BLF_N	16			/* Number of Subkeys */

#define F(s, x) ((((s)[        (((x)>>24)&0xFF)]  \
		 + (s)[0x100 + (((x)>>16)&0xFF)]) \
		 ^ (s)[0x200 + (((x)>> 8)&0xFF)]) \
		 + (s)[0x300 + ( (x)     &0xFF)])


#define BLFRND(s,p,i,j,n) (i ^= F(s,j) ^ (p)[n])

/* Blowfish context */
typedef struct BlowfishContext {
	u_int32_t S[4][256];	/* S-Boxes */
	u_int32_t P[BLF_N + 2];	/* Subkeys */
} blf_ctx;

void Blowfish_initstate(blf_ctx *c);
void Blowfish_expand0state(blf_ctx *c, const u_int8_t *key, u_int16_t keybytes);
void Blowfish_expandstate(blf_ctx *c, const u_int8_t *data, u_int16_t databytes,const u_int8_t *key, u_int16_t keybytes);
u_int32_t Blowfish_stream2word(const u_int8_t *data, u_int16_t databytes, u_int16_t *current);
void Blowfish_encipher(blf_ctx *c, u_int32_t *xl, u_int32_t *xr);
void blf_enc(blf_ctx *c, u_int32_t *data, u_int16_t blocks);