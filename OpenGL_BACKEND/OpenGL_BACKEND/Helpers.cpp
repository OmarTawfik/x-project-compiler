#include "Helpers.h"

int GetMilliTickCount()
{
    // Something like GetTickCount but portable
    // It rolls over every ~ 12.1 days (0x100000/24/60/60)
    // Use GetMilliSpan to correct for rollover
    timeb tb;
    ftime( &tb );
    int nCount = tb.millitm + (tb.time & 0xfffff) * 1000;
    return nCount;
}

char * MASReadFileToCharPointer(char *pFile)
{
    FILE * fp = fopen(pFile,"rb");
    if (fp == NULL) return 0;
    fseek(fp, 0, SEEK_END);
    long size = ftell(fp);
    fseek(fp, 0, SEEK_SET);
    char *pData = new char[size + 1];
    fread(pData, sizeof(char), size, fp);
    fclose(fp);
    pData[size] = '\0';
    return pData;
}