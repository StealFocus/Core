// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircularStream.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
//   http://msdn.microsoft.com/en-us/library/aa395205.aspx
// 
//   Defines the CircularStream type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.IO
{
    using System;
    using System.IO;
    
    /// <summary>
    /// CircularStream Class.
    /// </summary>
    public class CircularStream : Stream
    {
        /// <summary>
        /// Holds the file stream.
        /// </summary>
        private readonly FileStream[] fileStream;

        /// <summary>
        /// Holds the file paths.
        /// </summary>
        private readonly string[] filePaths;

        /// <summary>
        /// Holds the data.
        /// </summary>
        private long dataWritten;

        /// <summary>
        /// Holds the file size quota.
        /// </summary>
        private long fileQuota;

        /// <summary>
        /// Holds the .
        /// </summary>
        private int currentFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularStream"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public CircularStream(string fileName)
        {
            // Handle all exceptions within this class, since tracing shouldn't crash a service
            // Add 00 and 01 to FileNames and open streams
            try
            {
                string filePath = Path.GetDirectoryName(fileName);
                string fileBase = Path.GetFileNameWithoutExtension(fileName);
                string fileExt = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = AppDomain.CurrentDomain.BaseDirectory;
                }

                this.filePaths = new string[2];
                this.filePaths[0] = Path.Combine(filePath, fileBase + "-00" + fileExt);
                this.filePaths[1] = Path.Combine(filePath, fileBase + "-01" + fileExt);
                this.fileStream = new FileStream[2];
                this.fileStream[0] = new FileStream(this.filePaths[0], FileMode.Create);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Gets or sets MaxQuotaSize.
        /// </summary>
        public long MaxQuotaSize
        {
            get
            {
                return this.fileQuota;
            }

            set
            {
                this.fileQuota = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsOverQuota.
        /// </summary>
        public bool IsOverQuota
        {
            get
            {
                return this.dataWritten >= this.fileQuota;
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanRead.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                try
                {
                    return this.fileStream[this.currentFile].CanRead;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanSeek.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                try
                {
                    return this.fileStream[this.currentFile].CanSeek;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets Length.
        /// </summary>
        public override long Length
        {
            get
            {
                try
                {
                    return this.fileStream[this.currentFile].Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public override long Position
        {
            get
            {
                try
                {
                    return this.fileStream[this.currentFile].Position;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }

            set
            {
                try
                {
                    this.fileStream[this.currentFile].Position = this.Position;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanWrite.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                try
                {
                    return this.fileStream[this.currentFile].CanWrite;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return true;
                }
            }
        }

        /// <summary>
        /// Switch the files being used.
        /// </summary>
        public void SwitchFiles()
        {
            try
            {
                // Close current file, open next file (deleting its contents)
                this.dataWritten = 0;
                this.fileStream[this.currentFile].Close();
                this.currentFile = (this.currentFile + 1) % 2;
                this.fileStream[this.currentFile] = new FileStream(this.filePaths[this.currentFile], FileMode.Create);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Flish to the current file.
        /// </summary>
        public override void Flush()
        {
            try
            {
                this.fileStream[this.currentFile].Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Seek a position in the stream.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="origin">The origin.</param>
        /// <returns>The position of the stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            try
            {
                return this.fileStream[this.currentFile].Seek(offset, origin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Set the length of the current stream.
        /// </summary>
        /// <param name="value">The length to set.</param>
        public override void SetLength(long value)
        {
            try
            {
                this.fileStream[this.currentFile].SetLength(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Write to the stream.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                // Write to current file
                this.fileStream[this.currentFile].Write(buffer, offset, count);
                this.dataWritten += count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Read from the current stream.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>The value read.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                return this.fileStream[this.currentFile].Read(buffer, offset, count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Close the currrent stream.
        /// </summary>
        public override void Close()
        {
            try
            {
                this.fileStream[this.currentFile].Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
