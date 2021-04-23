using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using LitJson;

namespace JFrame
{
    

    public class SoundMGR : Singleton<SoundMGR>
    {

        List<CSoundInfo> SoundInfos;

        List<AudioSource> BufferBGM;
        List<AudioSource> BufferEFX;
        List<AudioSource> BufferVoice;


        const int MAX_BGM_CHANNEL = 1;
        const int MAX_EFX_CHANNEL = 3;
        const int MAX_VOICE_CHANNEL = 1;

        Dictionary<eChannel, int> _dictMaxChannelCount = new Dictionary<eChannel, int>
        {
            { eChannel.BGM, MAX_BGM_CHANNEL },
            { eChannel.EFX, MAX_EFX_CHANNEL },
            { eChannel.VOICE, MAX_VOICE_CHANNEL }
        };


        public enum eChannel
        {
            BGM = 0,
            EFX = 1,         
            VOICE = 2,
            MAX
        };

        public void Reset()
        {
            BufferBGM = new List<AudioSource>();
            BufferEFX = new List<AudioSource>();
            BufferVoice = new List<AudioSource>();

            //####  TODO : Load SoundInfos
            if (SoundInfos == null)
            {

            }

            StopSounds(eChannel.MAX);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) == true)
            {
                PlaySound("Fx_1GO_op");
            }
            if (Input.GetKeyDown(KeyCode.S) == true)
            {
                PlaySound("Fx_2GO_op");
            }
            if (Input.GetKeyDown(KeyCode.D) == true)
            {
                PlaySound("Fx_3GO_op");
            }

            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                StopSounds(eChannel.EFX);
            }
        }

        public void PlaySound (string _id)
        {
            CSoundInfo _info = GetSoundInfo(_id);

            _info = new CSoundInfo();
            _info.ID = _id;
            _info.ChannelType = (int)eChannel.EFX;
            _info.Path = _id;

            AudioClip _clip = Resources.Load<AudioClip>(_info.Path);
            if (_clip == null)
            {
                return;
            }


            //Find adequate audio source

            eChannel _channel = (eChannel)_info.ChannelType;

            List<AudioSource> _buffer = GetSoundBufferByChannel(_channel, true);
            if (_buffer == null)
            {
                //Wrong channel!
                return;
            }

            
            int _maxChannelCount = _dictMaxChannelCount.ContainsKey(_channel) ? _dictMaxChannelCount[_channel] : 1;


            //find audio src already used for same ID
            AudioSource _srcOnHierarcy = _buffer.Find(_item => _item.name == _id);
            if (_srcOnHierarcy != null)
            {
                _srcOnHierarcy.Stop();
                _srcOnHierarcy.Play();
                return;
            }
           

            if (_buffer.Count >= _maxChannelCount)
            {
                //Channel is full!
                //Release the oldest one and create new
                AudioSource _channelToRemove = _buffer[0];
                if (_channelToRemove != null)
                {
                    _channelToRemove.Stop();
                    Destroy(_channelToRemove.gameObject);
                }

                _buffer.RemoveAt(0);
            }


            //Create new audio source and add to buffer
            GameObject _goAudioSrc = new GameObject(_info.ID);
            _goAudioSrc.transform.parent = transform;
            _goAudioSrc.name = _info.ID;
            AudioSource _newChannel = _goAudioSrc.AddComponent<AudioSource>();

            _newChannel.clip = _clip;
            _newChannel.Play();
            _buffer.Add(_newChannel);
        }

        public void StopSound (eChannel _channel, string _id)
        {
            //need?
        }

        public void StopSounds (eChannel _channel)
        {
            //stop all sound @_channel

            // _channel : MAX => all channel
            // _bRealse : true => Clear Buffer 

            List<AudioSource>[] _buffers = GetSoundBuffersByChannel(_channel);

            for (int i = 0; i < _buffers.Length; i++)
            {
                List<AudioSource> _buffer = _buffers[i];

                if (_buffer == null)
                {
                    continue;
                }

                foreach (AudioSource _src in _buffer)
                {
                    if (_src != null)
                    {
                        _src.Stop();
                    }

                    Destroy(_src.gameObject);
                }
                _buffer = new List<AudioSource>();
            }
        }

        List<AudioSource> GetSoundBufferByChannel (eChannel _channel, bool _bInitWhenNull = false)
        {
            Dictionary<eChannel, List<AudioSource>> _dict = new Dictionary<eChannel, List<AudioSource>>()
            {
                { eChannel.BGM, BufferBGM },
                { eChannel.EFX, BufferEFX },
                { eChannel.VOICE, BufferVoice }
            };

            List<AudioSource> _ret = null;

            if (_dict.ContainsKey(_channel) == true)
            {
                _ret = _dict[_channel];

                if (_bInitWhenNull == true && _ret == null)
                {
                    _ret = new List<AudioSource>();
                }
            }

            return _ret;
        }

        List<AudioSource>[] GetSoundBuffersByChannel (eChannel _channel)
        {
            List<AudioSource>[] _ret = null;

            List<AudioSource> _bufferByChannel = GetSoundBufferByChannel(_channel);
            if (_bufferByChannel != null)
            {
                _ret = new List<AudioSource>[]
                {
                    _bufferByChannel
                };
            }
            else if (_channel == eChannel.MAX)
            {
                _ret = new List<AudioSource>[]
                {
                    BufferBGM, BufferEFX, BufferVoice
                };
            }

            return _ret;
        }

        CSoundInfo GetSoundInfo (string _id)
        {
            CSoundInfo _ret = null;

            if (SoundInfos != null && string.IsNullOrEmpty(_id) == false)
            {
                _ret = SoundInfos.Find(_item => string.Equals(_item.ID, _id, System.StringComparison.OrdinalIgnoreCase) == true);
            }


            return _ret;
            

        }


    }

    public class CSoundInfo
    {
        public string ID;
        public string Path;
        public bool IsLoop;
        public int ChannelType;
    }
    
}