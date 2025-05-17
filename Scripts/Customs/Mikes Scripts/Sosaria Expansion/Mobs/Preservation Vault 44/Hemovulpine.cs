using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // if you want to borrow visuals or spells
using Server.Spells.Sixth;   // for Blood field visuals, etc.

namespace Server.Mobiles
{
    [CorpseName("a hemovulpine corpse")]
    public class Hemovulpine : BaseCreature
    {
        private DateTime m_NextHematophageTime;
        private DateTime m_NextBloodRainTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextLeapTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1175; // deep crimson

        [Constructable]
        public Hemovulpine()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.3)
        {
            Name = "a Hemovulpine";
            Body = 0x58F;          // reuse BloodFox body
            Hue = UniqueHue;
            BaseSoundID = 0xE1;    // fox-like sounds

            // Stats
            SetStr(500, 600);
            SetDex(300, 350);
            SetInt(400, 450);

            SetHits(2000, 2300);
            SetStam(350, 400);
            SetMana(400, 500);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Cooldowns
            m_NextHematophageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextBloodRainTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextLeapTime        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Loot
            PackGold(2000, 3000);
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 25)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
            PackItem(new MaxxiaDust(Utility.RandomMinMax(5, 10))); // assume exists
        }
		
		public Hemovulpine(Serial serial) : base(serial) { }

        // --- Hematophage Aura (life drain on movement) ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (DateTime.UtcNow < m_NextHematophageTime)
                return;

            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && this.Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    int drain = Utility.RandomMinMax(15, 30);
                    AOS.Damage(target, this, drain, 0, 0, 0, 0, 100);
                    this.Hits = Math.Min(this.Hits + (drain / 2), this.HitsMax);

                    target.SendMessage(0x22, "You feel your life ebb away into the Hemovulpine!");
                    target.FixedParticles(0x375A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x15E);

                    m_NextHematophageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Blood Leap (teleport behind target)
            if (DateTime.UtcNow >= m_NextLeapTime && Combatant is Mobile leapTarget && InRange(leapTarget.Location, 12) && CanBeHarmful(leapTarget, false))
            {
                BloodLeap(leapTarget);
                m_NextLeapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Blood Rain (AoE bleed + damage around self)
            else if (DateTime.UtcNow >= m_NextBloodRainTime && InRange(Combatant.Location, 10))
            {
                BloodRain();
                m_NextBloodRainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Summon Blood Wraiths
            else if (DateTime.UtcNow >= m_NextSummonTime)
            {
                SummonBloodWraiths();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Teleport Strike ---
        private void BloodLeap(Mobile target)
        {
            this.Say("*The hunt begins anew!*");
            PlaySound(0x1FE);

            // Determine a point two steps behind the target
            int dx = target.X - this.X;
            int dy = target.Y - this.Y;
            int ox = dx != 0 ? dx / Math.Abs(dx) : 0;
            int oy = dy != 0 ? dy / Math.Abs(dy) : 0;
            Point3D dest = new Point3D(target.X + ox * 2, target.Y + oy * 2, target.Z);

            if (!Map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
            {
                dest.Z = Map.GetAverageZ(dest.X, dest.Y);
                if (!Map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
                    dest = target.Location;
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            this.MoveToWorld(dest, this.Map);
            Effects.SendLocationParticles(EffectItem.Create(dest, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);

            if (CanBeHarmful(target, false))
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
                target.SendMessage(0x22, "The Hemovulpine leaps upon you with savage force!");
                target.PlaySound(0x212);
            }
        }

        // --- Crimson Storm (AoE blood rain) ---
        private void BloodRain()
        {
            this.Say("*Bleed for me!*");
            PlaySound(0x21F);
            FixedParticles(0x376A, 1, 30, 9535, UniqueHue, 0, EffectLayer.Waist);

            List<Mobile> list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(25, 40);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);

                m.SendMessage(0x22, "Blood rains down upon you, searing and staining your flesh!");
                m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                m.PlaySound(0x1F5);

                if (Utility.RandomDouble() < 0.3 && m is Mobile targetMobile)
                {
                    targetMobile.ApplyPoison(this, Poison.Lethal);
                    targetMobile.SendMessage("You feel your veins seize with dark venom!");
                }
            }
        }

        // --- Summon Minions ---
        private void SummonBloodWraiths()
        {
            this.Say("*Rise, my children!*");
            PlaySound(0x1F7);

            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                BloodElemental w = new BloodElemental(); // assumes BloodWraith exists
                Point3D loc = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                w.MoveToWorld(loc, Map);
                w.Combatant = this.Combatant;
            }
        }

        // --- Death Effect ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My blood... will live on...*");
                PlaySound(0x212);
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
                {
                    Point3D p = new Point3D(
                        X + Utility.RandomMinMax(-2, 2),
                        Y + Utility.RandomMinMax(-2, 2),
                        Z
                    );

                    if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                        p.Z = Map.GetAverageZ(p.X, p.Y);

                    PoisonTile tile = new PoisonTile(); 
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(p, Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Misc ---
        public override int TreasureMapLevel { get { return 6; } }
        public override bool BleedImmune { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset cooldowns
            m_NextHematophageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextBloodRainTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextLeapTime        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
