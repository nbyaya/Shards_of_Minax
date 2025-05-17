using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a void rabbit corpse")]
    public class VoidRabbit : BaseCreature
    {
        private DateTime m_NextLeapTime;
        private DateTime m_NextGraspTime;
        private DateTime m_NextVolleyTime;
        private Point3D m_LastLocation;

        // A deep, otherworldly purple
        private const int VoidHue = 1175;

        [Constructable]
        public VoidRabbit()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Void Rabbit";
            Body = 0xCD;               // Jack Rabbit body
            Hue = VoidHue;

            // Base sounds inherited from Jack Rabbit
            BaseSoundID = 0; // we will override individual sounds

            // Stats
            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetStam(150, 180);
            SetMana(400, 500);

            SetDamage(12, 18);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.EvalInt, 100.0, 115.0);
            SetSkill(SkillName.Magery, 100.0, 115.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize timers
            var now = DateTime.UtcNow;
            m_NextLeapTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextGraspTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(5, 10)));
        }

        public override int GetAttackSound() { return 0xC9; }
        public override int GetHurtSound()   { return 0xCA; }
        public override int GetDeathSound()  { return 0xCB; }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && Alive && m.Map == this.Map && m.InRange(this.Location, 3) && CanBeHarmful(m, false))
            {
                // Void Aura: drains stamina
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int stamDrain = Utility.RandomMinMax(5, 12);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x44, "A chill of nothingness saps your strength!");
                        target.FixedParticles(0x377A, 10, 15, 5032, VoidHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x4F2);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Void Leap: teleport behind the target and strike
            if (now >= m_NextLeapTime && this.InRange(Combatant.Location, 12))
            {
                if (Combatant is Mobile target && CanBeHarmful(target, false))
                {
                    VoidLeap(target);
                    m_NextLeapTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
            }
            // Void Grasp: AoE life/mana drain
            else if (now >= m_NextGraspTime && this.InRange(Combatant.Location, 10))
            {
                VoidGrasp();
                m_NextGraspTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            // Carrot Volley: ranged projectile barrage
            else if (now >= m_NextVolleyTime)
            {
                CarrotVolley();
                m_NextVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void VoidLeap(Mobile target)
        {
            this.Say("*…*");
            PlaySound(0x1FE); // teleport sound

            // Teleport close to target
            Point3D dest = target.Location;
            dest.X += Utility.RandomMinMax(-1, 1);
            dest.Y += Utility.RandomMinMax(-1, 1);
            dest.Z = this.Z;
            this.MoveToWorld(dest, this.Map);

            // Visual effect
            target.FixedParticles(0x3728, 20, 10, 5042, VoidHue, 0, EffectLayer.Head);

            // Strike
            DoHarmful(target);
            int damage = Utility.RandomMinMax(60, 85);
            if (target is Mobile mTarget)
                AOS.Damage(mTarget, this, damage, 0, 0, 0, 0, 100);
        }

        private void VoidGrasp()
        {
            this.Say("*Embrace oblivion!*");
            PlaySound(0x228);

            List<Mobile> victims = new List<Mobile>();
            var eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                    victims.Add(target);
            }
            eable.Free();

            foreach (Mobile victim in victims)
            {
                DoHarmful(victim);

                // Drain health
                int hpDrain = Utility.RandomMinMax(20, 35);
                int actualDrain = Math.Min(hpDrain, victim.Hits - 1);
                victim.Hits -= actualDrain;
                this.Hits += actualDrain / 2; // heal self

                // Drain mana
                int manaDrain = Utility.RandomMinMax(15, 25);
                if (victim.Mana >= manaDrain)
                {
                    victim.Mana -= manaDrain;
                    this.Mana += manaDrain / 2;
                }

                // Effects
                victim.FixedParticles(0x3709, 10, 30, 5032, VoidHue, 0, EffectLayer.Waist);
                victim.PlaySound(0x231);
            }
        }

        private void CarrotVolley()
        {
            this.Say("*Snack time!*");
            PlaySound(0x1E3);

            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false))
                return;

            var targets = new List<Mobile>();
            targets.Add(initial);

            // Gather up to 4 extra nearby targets
            var eable = Map.GetMobilesInRange(initial.Location, 8);
            foreach (Mobile m in eable)
            {
                if (targets.Count >= 5) break;
                if (m != this && m != initial && CanBeHarmful(m, false) && m is Mobile candidate)
                    targets.Add(candidate);
            }
            eable.Free();

            foreach (var tgt in targets)
            {
                // Launch ethereal carrot projectile
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, tgt.Location, this.Map),
                    0x1FAF,  // carrot graphic
                    7, 0, false, false, VoidHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
                {
                    if (CanBeHarmful(tgt, false) && tgt is Mobile mb)
                    {
                        DoHarmful(mb);
                        int dmg = Utility.RandomMinMax(20, 30);
                        AOS.Damage(mb, this, dmg, 0, 0, 0, 0, 100);
                        mb.FixedParticles(0x374A, 5, 15, 9502, VoidHue, 0, EffectLayer.Waist);
                        mb.SendMessage(0x2A, "You are struck by a spectral carrot shard!");
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            // Death explosion
            this.Say("*…void returns…*");
            PlaySound(0x1FE);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 15, 60, VoidHue, 0, 5052, 0);

            // Scatter void tiles
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new VortexTile();
                tile.Hue = VoidHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);

                Effects.SendLocationParticles(EffectItem.Create(new Point3D(x, y, z), Map, EffectItem.DefaultDuration), 0x376A, 8, 20, VoidHue, 0, 5039, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 1);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // 5% chance for Void Core
            if (Utility.RandomDouble() < 0.05)
                PackItem(new VoidCore());
        }

        public override bool BleedImmune { get { return true; } }
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus     { get { return 60.0; } }
        public override int TreasureMapLevel   { get { return 5; } }

        public VoidRabbit(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
