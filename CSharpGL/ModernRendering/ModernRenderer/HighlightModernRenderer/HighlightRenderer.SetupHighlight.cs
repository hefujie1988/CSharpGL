﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGL
{
    public partial class HighlightRenderer
    {
        /// <summary>
        /// 清空高亮显示。
        /// </summary>
        public void ClearHighlightIndexes()
        {
            var indexBufferPtr = this.indexBufferPtr as OneIndexBufferPtr;
            indexBufferPtr.ElementCount = 0;
        }

        /// <summary>
        /// 设置要高亮显示的图元。
        /// </summary>
        /// <param name="mode">要高亮显示的图元类型</param>
        /// <param name="indexes">要高亮显示的图元的索引。</param>
        public void SetHighlightIndexes(DrawMode mode, params uint[] indexes)
        {
            var indexBufferPtr = this.indexBufferPtr as OneIndexBufferPtr;
            int indexesLength = indexes.Length;
            if (indexesLength > this.maxElementCount)
            {
                using (var buffer = new OneIndexBuffer<uint>(
                    indexBufferPtr.Mode, BufferUsage.DynamicDraw))
                {
                    buffer.Alloc(indexesLength);
                    indexBufferPtr = buffer.GetBufferPtr() as OneIndexBufferPtr;
                }
                this.maxElementCount = indexesLength;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferPtr.BufferId);
            IntPtr pointer = GL.MapBuffer(BufferTarget.ElementArrayBuffer, MapBufferAccess.WriteOnly);
            unsafe
            {
                var array = (uint*)pointer.ToPointer();
                for (int i = 0; i < indexesLength; i++)
                {
                    array[i] = indexes[i];
                }
            }
            GL.UnmapBuffer(BufferTarget.ElementArrayBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            indexBufferPtr.Mode = mode;
            indexBufferPtr.ElementCount = indexesLength;
        }

    }


}
