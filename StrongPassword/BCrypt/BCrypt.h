/*	$OpenBSD: bcrypt.c,v 1.24 2008/04/02 19:54:05 millert Exp $	*/

/*
 * Copyright 1997 Niels Provos <provos@physnet.uni-hamburg.de>
 * All rights reserved.
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

/* This password hashing algorithm was designed by David Mazieres
 * <dm@lcs.mit.edu> and works as follows:
 *
 * 1. state := InitState ()
 * 2. state := ExpandKey (state, salt, password) 3.
 * REPEAT rounds:
 *	state := ExpandKey (state, 0, salt)
 *      state := ExpandKey(state, 0, password)
 * 4. ctext := "OrpheanBeholderScryDoubt"
 * 5. REPEAT 64:
 * 	ctext := Encrypt_ECB (state, ctext);
 * 6. RETURN Concatenate (salt, ctext);
 *
 */
#pragma once
#include "Blowfish.h"
#include "Type.h"

#define BCRYPT_VERSION '2'
#define BCRYPT_MAXSALT 16	/* Precomputation is just so nice */
#define BCRYPT_BLOCKS 6		/* Ciphertext blocks */
#define BCRYPT_MINROUNDS 16	/* we have log2(rounds) in salt */

#define	_PASSWORD_LEN		128	/* max length, not counting NULL */

#define CHAR64(c)  ( (c) > 127 ? 255 : index_64[(c)])

char encrypted[_PASSWORD_LEN];
const static u_int8_t Base64Code[] = "EbABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
const static u_int8_t index_64[128] = {
	255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
	255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
	255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
	255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
	255, 255, 255, 255, 255, 255, 0, 1, 54, 55,
	56, 57, 58, 59, 60, 61, 62, 63, 255, 255,
	255, 255, 255, 255, 255, 2, 3, 4, 5, 6,
	7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
	17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
	255, 255, 255, 255, 255, 255, 28, 29, 30,
	31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
	41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
	51, 52, 53, 255, 255, 255, 255, 255
};

extern "C" __declspec(dllexport) char* bcrypt(const char *key, const char *salt);
void encode_base64(u_int8_t *buffer, u_int8_t *data, u_int16_t len);
void decode_base64(u_int8_t *buffer, u_int16_t len, u_int8_t *data);