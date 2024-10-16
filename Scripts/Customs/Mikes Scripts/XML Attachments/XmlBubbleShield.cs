using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBubbleShield : XmlAttachment
    {
        private DateTime m_BubbleShieldEnd;
        private DateTime m_NextBubbleBurst;
        private DateTime m_NextBubbleShield;
        private Timer m_ThinkTimer;

        [Attachable]
        public XmlBubbleShield() 
        {
            StartThinking();
        }

        [Attachable]
        public XmlBubbleShield(double duration, double refractory)
        {
            m_BubbleShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(duration);
            m_NextBubbleShield = DateTime.UtcNow + TimeSpan.FromSeconds(refractory);
            StartThinking();
        }

        private void StartThinking()
        {
            m_ThinkTimer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), OnThink);
        }

        private void StopThinking()
        {
            if (m_ThinkTimer != null)
            {
                m_ThinkTimer.Stop();
                m_ThinkTimer = null;
            }
        }

        private void OnThink()
        {
            if (DateTime.UtcNow >= m_BubbleShieldEnd && m_BubbleShieldEnd != DateTime.MinValue)
            {
                DeactivateBubbleShield();
            }

            if (Utility.RandomDouble() < 0.1)
            {
                BlowBubbles();
            }

            if (DateTime.UtcNow >= m_NextBubbleBurst)
            {
                DoBubbleBurst();
            }
        }

        private void DeactivateBubbleShield()
        {
            if (AttachedTo is Mobile m && m_BubbleShieldEnd != DateTime.MinValue)
            {
                m.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Shield Fades *");
                m_BubbleShieldEnd = DateTime.MinValue;
            }
        }

        private void ActivateBubbleShield()
        {
            if (AttachedTo is Mobile m)
            {
                m.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Shield *");
                m.PlaySound(0x1E3);

                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                m_BubbleShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                m_NextBubbleShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void BlowBubbles()
        {
            if (AttachedTo is Mobile m)
            {
                m.MovingParticles(m, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
            }
        }

        private void DoBubbleBurst()
        {
            if (AttachedTo is Mobile m && m.Map != null)
            {
                m.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Burst *");
                m.PlaySound(0x026);

                foreach (Mobile mobile in m.Map.GetMobilesInRange(m.Location, 3))
                {
                    if (mobile != m && mobile.Alive && m.CanBeHarmful(mobile))
                    {
                        m.DoHarmful(mobile);
                        AOS.Damage(mobile, m, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Cold damage
                        mobile.SendLocalizedMessage(1114727);
                        mobile.MovingParticles(m, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
                    }
                }

                m_NextBubbleBurst = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        public void OnMeleeDamageTaken(Mobile from, ref int damage)
        {
            if (m_BubbleShieldEnd > DateTime.UtcNow)
            {
                damage = 0;
                from.SendLocalizedMessage(1114728);
            }
        }
    }
}
