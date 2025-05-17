using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a mournful vapor corpse")]
    public class MournfulVapors : BaseCreature, IAuraCreature
    {
        private DateTime m_NextWail;
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextMemoryLeech;

        [Constructable]
        public MournfulVapors()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Mournful Vapors";
            Body = 0x104; // Same as Corporeal Brume
            BaseSoundID = 0x56B;
            Hue = 2601; // Unique ghostly blue-green mist

            SetStr(600, 650);
            SetDex(150, 200);
            SetInt(400, 450);

            SetHits(1800, 2000);
            SetDamage(30, 35);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 120.0, 125.0);
            SetSkill(SkillName.Tactics, 120.0, 125.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);

            Fame = 28000;
            Karma = -28000;

            VirtualArmor = 100;
        }

        public MournfulVapors(Serial serial)
            : base(serial)
        {
        }
        public override bool CanBeParagon => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.Gems, 10);
            if (Utility.RandomDouble() < 0.002)
                PackItem(new WhisperboundCowl()); // Rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive) return;

            if (DateTime.UtcNow >= m_NextWail)
                SpectralWail();

            if (DateTime.UtcNow >= m_NextPhantomStrike)
                PhantomStrike();

            if (DateTime.UtcNow >= m_NextMemoryLeech)
                MemoryLeech();
        }

        public void AuraEffect(Mobile m)
        {
            m.FixedParticles(0x374A, 10, 15, 5038, 2052, 2, EffectLayer.Head);
            m.PlaySound(0x213);
            m.SendMessage("A wave of chilling sorrow drains your strength...");
            m.Damage(Utility.RandomMinMax(5, 15), this);
        }

        private void SpectralWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x9C2, true, "*The Mournful Vapors releases a soul-piercing wail!*");
            PlaySound(0x29B); // Spectral scream

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && !m.Hidden)
                {
                    m.SendMessage("The wail stuns you with overwhelming grief!");
                    m.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => { if (m != null) m.Frozen = false; });
                }
            }

            m_NextWail = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PhantomStrike()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x9C2, true, "*The vapor vanishes and reappears behind its target!*");
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                MoveToWorld(Combatant.Location, Combatant.Map);
                Combatant.Damage(Utility.RandomMinMax(25, 35), this);
                PlaySound(0x64F); // Attack whoosh
            }

            m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void MemoryLeech()
        {
            Mobile target = Combatant as Mobile;
            if (target == null) return;

            target.SendMessage("You feel your thoughts slipping away...");
            PlaySound(0x1ED);
            target.FixedParticles(0x376A, 9, 32, 5035, EffectLayer.Head);
            target.Paralyzed = true;
            Timer.DelayCall(TimeSpan.FromSeconds(2), () => target.Paralyzed = false);

            int manaDrain = Utility.RandomMinMax(20, 40);
            target.Mana = Math.Max(0, target.Mana - manaDrain);
            this.Mana = Math.Min(this.Mana + manaDrain, this.ManaMax);

            m_NextMemoryLeech = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WhisperboundCowl : BaseHat
    {
        public override int LabelNumber => 1151352; // Whisperbound Cowl

        [Constructable]
        public WhisperboundCowl() : base(0x1540)
        {
            Hue = 2601;
            LootType = LootType.Regular;
            Name = "Whisperbound Cowl";
        }

        public WhisperboundCowl(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
