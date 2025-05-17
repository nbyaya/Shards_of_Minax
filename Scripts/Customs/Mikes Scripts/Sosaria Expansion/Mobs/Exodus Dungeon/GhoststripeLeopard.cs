using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a ghoststripe leopard corpse")]
    public class GhoststripeLeopard : BaseCreature
    {
        private DateTime m_NextPhantomPounce;
        private DateTime m_NextMirrorHowl;
        private DateTime m_NextSpiritFade;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GhoststripeLeopard()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a ghoststripe leopard";
            Body = Utility.RandomList(64, 65); // Same as Snow Leopard
            BaseSoundID = 0x73;
            Hue = 1153; // Ethereal icy blue/white glow

            SetStr(200, 280);
            SetDex(180, 240);
            SetInt(120, 150);

            SetHits(700, 900);
            SetMana(200);
            SetStam(200);

            SetDamage(14, 22);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 105.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.SpiritSpeak, 60.0, 80.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.02) // 2% rare drop
                PackItem(new GhoststripeHide());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextPhantomPounce = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 12));
                    m_NextMirrorHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                    m_NextSpiritFade = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPhantomPounce)
                    PhantomPounce();

                if (DateTime.UtcNow >= m_NextMirrorHowl)
                    MirrorHowl();

                if (DateTime.UtcNow >= m_NextSpiritFade)
                    SpiritFade();
            }
        }

        private void PhantomPounce()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x47E, true, "*The Ghoststripe Leopard vanishes and reappears mid-pounce!*");
                Effects.SendLocationEffect(Location, Map, 0x3728, 10, 1);

                MoveToWorld(target.Location, Map);
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);

                target.SendMessage(0x22, "You feel the chill of another realm as claws rake across you!");
            }

            m_NextPhantomPounce = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void MirrorHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47E, true, "*The Ghoststripe Leopard lets out a haunting howl that echoes beyond time...*");
            PlaySound(BaseSoundID);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile mobile && mobile.Combatant == this)
                {
                    mobile.SendMessage(0x22, "Your vision fractures as mirror images assault your mind!");
                    mobile.FixedParticles(0x373A, 10, 15, 5018, 0x48F, 2, EffectLayer.Head);
                    mobile.PlaySound(0x1F7);
                    mobile.Paralyze(TimeSpan.FromSeconds(2.0 + Utility.RandomDouble() * 2));
                }
            }

            m_NextMirrorHowl = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.RandomDouble() * 15);
        }

        private void SpiritFade()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47E, true, "*The Ghoststripe Leopard slips into the spirit realm...*");
            Hidden = true;
            Blessed = true;
            Combatant = null;

            Timer.DelayCall(TimeSpan.FromSeconds(6), () =>
            {
                PublicOverheadMessage(MessageType.Regular, 0x47E, true, "*The Ghoststripe Leopard reemerges with a ghostly growl!*");
                Hidden = false;
                Blessed = false;
            });

            m_NextSpiritFade = DateTime.UtcNow + TimeSpan.FromSeconds(60 + Utility.RandomDouble() * 20);
        }

        public GhoststripeLeopard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class GhoststripeHide : Item
    {
        [Constructable]
        public GhoststripeHide() : base(0x11F5)
        {
            Name = "a ghoststripe hide";
            Hue = 1153;
            Weight = 2.0;
        }

        public GhoststripeHide(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
