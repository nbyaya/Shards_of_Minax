using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a phantom trickster corpse")]
    public class PhantomTrickster : BaseCreature
    {
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextMirrorImage;
        private DateTime m_NextPossession;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public PhantomTrickster()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Phantom Trickster";
            Body = 729; // Mimic-like form
            Hue = 2610; // Ethereal violet/ghostly hue
            BaseSoundID = 1561;

            SetStr(400, 550);
            SetDex(200, 250);
            SetInt(350, 450);

            SetHits(900, 1100);
            SetMana(1500);

            SetDamage(15, 25);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 100); // Immune
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 75;
            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 12));
                    m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_NextPossession = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPhantomStrike)
                    PhantomStrike();

                if (DateTime.UtcNow >= m_NextMirrorImage)
                    MirrorImage();

                if (DateTime.UtcNow >= m_NextPossession)
                    PossessionAttempt();
            }
        }

        private void PhantomStrike()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x482, true, "* The Trickster vanishes and strikes from the shadows! *");
                PlaySound(1558); // Attack sound

                this.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                {
                    this.Hidden = false;
                    Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 10, 1);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 0, 100, 0, 0);

                    target.SendMessage(0x22, "You feel your soul shudder as it strikes.");
                });

                m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void MirrorImage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Trickster splits into reflections! *");

            int numberOfClones = 2 + Utility.Random(2);

            for (int i = 0; i < numberOfClones; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    PhantomClone clone = new PhantomClone(this);
                    clone.MoveToWorld(this.Location, this.Map);
                });
            }

            m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void PossessionAttempt()
        {
            if (Combatant is Mobile target && Utility.RandomDouble() < 0.4)
            {
                PublicOverheadMessage(MessageType.Regular, 0x482, true, "* The Trickster whispers into your mind... *");
                target.SendMessage(0x22, "A voice tries to override your thoughts...");

                target.Freeze(TimeSpan.FromSeconds(3));
                Effects.SendLocationEffect(target.Location, target.Map, 0x375A, 20);
                target.PlaySound(0x5C9); // Mind attack

                if (Utility.RandomDouble() < 0.33)
                {
                    target.SendMessage(0x22, "You briefly feel like someone else...");
                    target.AddStatMod(new StatMod(StatType.Int, "TrickInt", -10, TimeSpan.FromMinutes(1)));
                    target.AddStatMod(new StatMod(StatType.Dex, "TrickDex", -10, TimeSpan.FromMinutes(1)));
                    target.AddStatMod(new StatMod(StatType.Str, "TrickStr", -10, TimeSpan.FromMinutes(1)));
                }
            }

            m_NextPossession = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
                c.DropItem(new PhantomMask());

            if (Utility.RandomDouble() < 0.03)
                c.DropItem(new IllusionaryEssence());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 3);
            AddLoot(LootPack.Gems, 5);
        }

        public override int GetIdleSound() => 1561;
        public override int GetAngerSound() => 1558;
        public override int GetHurtSound() => 1560;
        public override int GetDeathSound() => 1559;

        public PhantomTrickster(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class PhantomClone : BaseCreature
    {
        private Timer m_Timer;

        public PhantomClone(BaseCreature parent)
            : base(AIType.AI_Melee, FightMode.None, 1, 1, 0.2, 0.4)
        {
            Name = "a phantom image";
            Body = parent.Body;
            Hue = 2610;
            Hidden = false;
            CantWalk = false;
            BaseSoundID = parent.BaseSoundID;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(1);

            VirtualArmor = 0;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete);
        }

        public override void OnDelete()
        {
            m_Timer?.Stop();
            base.OnDelete();
        }


        public PhantomClone(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete);
        }
    }

    public class PhantomMask : Item
    {
        public PhantomMask() : base(0x141B)
        {
            Name = "Mask of the Phantom";
            Hue = 2610;
            LootType = LootType.Blessed;
        }

        public PhantomMask(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class IllusionaryEssence : Item
    {
        public IllusionaryEssence() : base(0xF8D)
        {
            Name = "Illusionary Essence";
            Hue = 2610;
            Weight = 1.0;
        }

        public IllusionaryEssence(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
