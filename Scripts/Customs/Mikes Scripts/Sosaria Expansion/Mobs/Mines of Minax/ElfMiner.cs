using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an elf miner corpse")]
    public class ElfMiner : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextTremorTime;
        private DateTime m_NextShrapnelTime;
        private DateTime m_NextGasTime;
        private Point3D m_LastLocation;

        // Unique rock‐tinted hue
        private const int UniqueHue = 2610; // A slate‐gray tone

        [Constructable]
        public ElfMiner()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Race = Race.Elf;

            // Body & Name based on ElfBrigand
            if (Female = Utility.RandomBool())
            {
                Body = 606;
                Name = NameList.RandomName("Elf female");
            }
            else
            {
                Body = 605;
                Name = NameList.RandomName("Elf male");
            }

            Title = "the miner";
            Hue = UniqueHue;
            BaseSoundID = 0xA8; // same elf miner chime

            // --- Stats ---
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(800, 900);
            SetStam(150, 200);
            SetMana(100, 150);

            SetDamage(20, 30);

            // --- Damage Types ---
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 10);
            SetDamageType(ResistanceType.Poison, 10);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Skills ---
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Mining, 100.0, 120.0);

            Fame = 10000;
            Karma = -5000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // --- Ability cooldowns ---
            m_NextTremorTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShrapnelTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextGasTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            // --- Loot: ores, gems, pickaxes ---
            PackItem(new IronOre(Utility.RandomMinMax(10, 20)));
            PackItem(new GoldOre(Utility.RandomMinMax(5, 10)));
            PackGem();
            PackItem(new Pickaxe()); 

            m_LastLocation = this.Location;
        }

        public ElfMiner(Serial serial)
            : base(serial)
        {
        }

        // Leave hidden landmines behind as he moves
        public override void OnThink()
        {
            base.OnThink();

            // Movement effect: leave LandmineTiles
            if (this.Map != null && this.Location != m_LastLocation && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                // Attempt to place landmine at old location
                if (!this.Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                    oldLoc.Z = this.Map.GetAverageZ(oldLoc.X, oldLoc.Y);

                LandmineTile mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(oldLoc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // Do nothing else if no opponent yet
            if (Combatant == null || !Alive || this.Map == null || this.Map == Map.Internal)
                return;

            // --- Special Attacks ---
            if (DateTime.UtcNow >= m_NextShrapnelTime && InRange(Combatant.Location, 12))
            {
                ShrapnelBurstAttack();
                m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            else if (DateTime.UtcNow >= m_NextTremorTime && InRange(Combatant.Location, 8))
            {
                EarthTremorAttack();
                m_NextTremorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextGasTime)
            {
                GasVentAttack();
                m_NextGasTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Shrapnel Burst: Ranged physical + minor poison ---
        private void ShrapnelBurstAttack()
        {
            if (!(Combatant is Mobile primary)) return;

            Say("*Feel the sting of rock!*");
            PlaySound(0x2F3); // rock break sound

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.Map.GetMobilesInRange(this.Location, 12))
            {
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                Mobile target = targets[i];
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x1BFB, 7, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int phys = Utility.RandomMinMax(20, 40);
                        int pois = Utility.RandomMinMax(5, 15);
                        AOS.Damage(target, this, phys + pois, phys, 0, 0, pois, 0);
                    }
                });
            }
        }

        // --- Earth Tremor: Stun & AoE damage ---
        private void EarthTremorAttack()
        {
            Say("*The ground trembles!*");
            PlaySound(0x20F); // quake rumble
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 10, 30, UniqueHue, 0, 5032, 0);

            foreach (Mobile m in this.Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile targetMobile)
                {
                    DoHarmful(targetMobile);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(targetMobile, this, dmg, 100, 0, 0, 0, 0);

                    // Brief stun
                    targetMobile.Freeze(TimeSpan.FromSeconds(2.0));
                    targetMobile.SendMessage("You are shaken and off‐balance!");
                }
            }
        }

        // --- Gas Vent: Plant a lingering poison cloud at the foe's feet ---
        private void GasVentAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Toxic fumes!*");
            PlaySound(0x23D); // hiss

            Point3D loc = target.Location;
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return;

                if (!this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = this.Map.GetAverageZ(loc.X, loc.Y);

                PoisonTile cloud = new PoisonTile();
                cloud.Hue = UniqueHue;
                cloud.MoveToWorld(loc, this.Map);
            });
        }

        // --- On Death: erupt hazardous tiles around corpse ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (this.Map == null) return;
            Say("*The mine claims you!*");
            PlaySound(0x11B);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0);

            // Scatter rockfall hazards
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                Point3D pos = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(pos.X, pos.Y, pos.Z, 16, false, false))
                    pos.Z = Map.GetAverageZ(pos.X, pos.Y);

                EarthquakeTile quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(pos, this.Map);
            }
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));
            if (Utility.RandomDouble() < 0.05) // 5% chance for special pickaxe
                PackItem(new TombwoodKnocker()); 
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers
            m_NextTremorTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextGasTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation     = this.Location;
        }
    }
}
